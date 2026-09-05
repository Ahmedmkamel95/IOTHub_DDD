using CIOT.Common.CQRS;
using CIOT.Common.Results;
using CIOT.Modules.Asset.Application.Contracts;
using CIOT.Modules.Asset.Application.Dtos;
using CIOT.Modules.Asset.Domain;
using CIOT.Modules.Asset.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.Asset.Application.Commands;

public record RegisterAssetCommand(RegisterAssetRequest Request) : ICommand<AssetDto>;
public record AssignAssetToOutletCommand(Guid AssetId, AssignAssetToOutletRequest Request) : ICommand<AssetOutletAssignmentDto>;
public record AssignAssetToCustomerOutletCommand(Guid AssetId, AssignAssetToCustomerOutletRequest Request) : ICommand<AssetOutletAssignmentDto>;

public class RegisterAssetCommandValidator : AbstractValidator<RegisterAssetCommand>
{
    public RegisterAssetCommandValidator()
    {
        RuleFor(x => x.Request.SapEquipmentNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Request.CountryCode).NotEmpty().MaximumLength(10);
    }
}

public class AssignAssetToCustomerOutletCommandValidator : AbstractValidator<AssignAssetToCustomerOutletCommand>
{
    public AssignAssetToCustomerOutletCommandValidator()
    {
        RuleFor(x => x.AssetId).NotEmpty();
        RuleFor(x => x.Request.CustomerId).NotEmpty();
    }
}

public class AssetCommandHandlers :
    IRequestHandler<RegisterAssetCommand, Result<AssetDto>>,
    IRequestHandler<AssignAssetToOutletCommand, Result<AssetOutletAssignmentDto>>
{
    private readonly AssetDbContext _dbContext;

    public AssetCommandHandlers(AssetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<AssetDto>> Handle(RegisterAssetCommand command, CancellationToken cancellationToken)
    {
        var req = command.Request;
        var existing = await _dbContext.Assets.AnyAsync(a => a.SapEquipmentNumber == req.SapEquipmentNumber, cancellationToken);
        if (existing)
        {
            return Result.Failure<AssetDto>(Error.Conflict("Asset.Duplicate", $"Asset with SAP Equipment Number '{req.SapEquipmentNumber}' already exists."));
        }

        var asset = new Domain.Asset
        {
            SapEquipmentNumber = req.SapEquipmentNumber,
            OemSerialNumber = req.OemSerialNumber,
            TechnicalId = req.TechnicalId,
            EquipmentModelId = req.EquipmentModelId,
            CountryCode = req.CountryCode.ToUpperInvariant(),
            SapStatus = req.SapStatus,
            IsActive = true
        };

        _dbContext.Assets.Add(asset);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new AssetDto(asset.Id, asset.SapEquipmentNumber, asset.OemSerialNumber, asset.TechnicalId, asset.EquipmentModelId, asset.CountryCode, asset.SapStatus, asset.IsActive, null, asset.LastConnectionAtUtc));
    }

    public async Task<Result<AssetOutletAssignmentDto>> Handle(AssignAssetToOutletCommand command, CancellationToken cancellationToken)
    {
        var asset = await _dbContext.Assets
            .Include(a => a.OutletAssignments)
            .FirstOrDefaultAsync(a => a.Id == command.AssetId, cancellationToken);

        if (asset == null)
        {
            return Result.Failure<AssetOutletAssignmentDto>(Error.NotFound("Asset.NotFound", $"Asset '{command.AssetId}' not found."));
        }

        var newAssignment = asset.AssignToCustomerOutlet(command.Request.CustomerId, command.Request.OutletId);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new AssetOutletAssignmentDto(
            newAssignment.Id,
            newAssignment.AssetId,
            newAssignment.OutletId,
            newAssignment.CustomerId,
            newAssignment.AssignedAtUtc,
            newAssignment.UnassignedAtUtc,
            newAssignment.IsCurrent));
    }
}

public class AssignAssetToCustomerOutletCommandHandler : IRequestHandler<AssignAssetToCustomerOutletCommand, Result<AssetOutletAssignmentDto>>
{
    private readonly AssetDbContext _dbContext;
    private readonly ICustomerOutletValidator _validator;

    public AssignAssetToCustomerOutletCommandHandler(AssetDbContext dbContext, ICustomerOutletValidator validator)
    {
        _dbContext = dbContext;
        _validator = validator;
    }

    public async Task<Result<AssetOutletAssignmentDto>> Handle(AssignAssetToCustomerOutletCommand command, CancellationToken cancellationToken)
    {
        var asset = await _dbContext.Assets
            .Include(a => a.OutletAssignments)
            .FirstOrDefaultAsync(a => a.Id == command.AssetId, cancellationToken);

        if (asset == null)
        {
            return Result.Failure<AssetOutletAssignmentDto>(Error.NotFound("Asset.NotFound", $"Asset '{command.AssetId}' not found."));
        }

        // 1. Cross-module verification: check if Customer is valid, active, and cluster is active
        var validationResult = await _validator.ValidateCustomerAndClusterAsync(
            command.Request.CustomerId,
            command.Request.OutletId,
            command.Request.ClusterId,
            cancellationToken);

        if (!validationResult.IsSuccess)
        {
            return Result.Failure<AssetOutletAssignmentDto>(validationResult.Error);
        }

        // 2. Aggregate root enforces invariants
        var newAssignment = asset.AssignToCustomerOutlet(command.Request.CustomerId, command.Request.OutletId);
        _dbContext.AssetOutletAssignments.Add(newAssignment);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new AssetOutletAssignmentDto(
            newAssignment.Id,
            newAssignment.AssetId,
            newAssignment.OutletId,
            newAssignment.CustomerId,
            newAssignment.AssignedAtUtc,
            newAssignment.UnassignedAtUtc,
            newAssignment.IsCurrent));
    }
}
