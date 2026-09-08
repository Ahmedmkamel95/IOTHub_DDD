using CIOT.Common.CQRS;
using CIOT.Common.Contracts.CustomerOutlet;
using CIOT.Common.Results;
using CIOT.Modules.Asset.Application.Dtos;
using CIOT.Modules.Asset.Domain.Entities;
using CIOT.Modules.Asset.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AssetEntity = CIOT.Modules.Asset.Domain.Entities.Asset;

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

        var asset = new AssetEntity
        {
            SapEquipmentNumber = req.SapEquipmentNumber,
            OemSerialNumber = req.OemSerialNumber,
            TechnicalId = req.TechnicalId,
            EquipmentModelId = req.EquipmentModelId,
            CountryCode = req.CountryCode.ToUpperInvariant(),
            Status = req.SapStatus ?? "active"
        };

        _dbContext.Assets.Add(asset);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new AssetDto(asset.Id, asset.SapEquipmentNumber, asset.OemSerialNumber, asset.TechnicalId, asset.EquipmentModelId, asset.CountryCode, asset.Status, string.Equals(asset.Status, "active", StringComparison.OrdinalIgnoreCase), asset.CurrentOutletId, asset.LastConnectionAtUtc?.UtcDateTime));
    }

    public async Task<Result<AssetOutletAssignmentDto>> Handle(AssignAssetToOutletCommand command, CancellationToken cancellationToken)
    {
        var asset = await _dbContext.Assets.FirstOrDefaultAsync(a => a.Id == command.AssetId, cancellationToken);

        if (asset == null)
        {
            return Result.Failure<AssetOutletAssignmentDto>(Error.NotFound("Asset.NotFound", $"Asset '{command.AssetId}' not found."));
        }

        var newAssignment = new AssetOutletAssignment { AssetId = asset.Id, CustomerId = command.Request.CustomerId, OutletId = command.Request.OutletId, AssignedAtUtc = DateTimeOffset.UtcNow, CreatedAtUtc = DateTimeOffset.UtcNow };
        asset.CurrentOutletId = command.Request.OutletId;
        _dbContext.AssetOutletAssignments.Add(newAssignment);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new AssetOutletAssignmentDto(
            newAssignment.Id,
            newAssignment.AssetId,
            newAssignment.OutletId,
            newAssignment.CustomerId,
            newAssignment.AssignedAtUtc.UtcDateTime,
            newAssignment.RemovedAtUtc?.UtcDateTime,
            newAssignment.RemovedAtUtc == null));
    }
}

public class AssignAssetToCustomerOutletCommandHandler : IRequestHandler<AssignAssetToCustomerOutletCommand, Result<AssetOutletAssignmentDto>>
{
    private readonly AssetDbContext _dbContext;
    private readonly ICustomerOutletApi _customerOutletApi;

    public AssignAssetToCustomerOutletCommandHandler(AssetDbContext dbContext, ICustomerOutletApi customerOutletApi)
    {
        _dbContext = dbContext;
        _customerOutletApi = customerOutletApi;
    }

    public async Task<Result<AssetOutletAssignmentDto>> Handle(AssignAssetToCustomerOutletCommand command, CancellationToken cancellationToken)
    {
        var asset = await _dbContext.Assets.FirstOrDefaultAsync(a => a.Id == command.AssetId, cancellationToken);

        if (asset == null)
        {
            return Result.Failure<AssetOutletAssignmentDto>(Error.NotFound("Asset.NotFound", $"Asset '{command.AssetId}' not found."));
        }

        // 1. Cross-module verification: check if Customer is valid, active, and cluster is active
        var validationResult = await _customerOutletApi.ValidateCustomerAndClusterAsync(
            command.Request.CustomerId,
            command.Request.OutletId,
            command.Request.ClusterId,
            cancellationToken);

        if (!validationResult.IsSuccess)
        {
            return Result.Failure<AssetOutletAssignmentDto>(validationResult.Error);
        }

        var activeAssignments = await _dbContext.AssetOutletAssignments.Where(x => x.AssetId == asset.Id && x.RemovedAtUtc == null).ToListAsync(cancellationToken);
        foreach (var assignment in activeAssignments) assignment.RemovedAtUtc = DateTimeOffset.UtcNow;
        var newAssignment = new AssetOutletAssignment { AssetId = asset.Id, CustomerId = command.Request.CustomerId, OutletId = command.Request.OutletId, AssignedAtUtc = DateTimeOffset.UtcNow, CreatedAtUtc = DateTimeOffset.UtcNow };
        asset.CurrentOutletId = command.Request.OutletId;
        _dbContext.AssetOutletAssignments.Add(newAssignment);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new AssetOutletAssignmentDto(
            newAssignment.Id,
            newAssignment.AssetId,
            newAssignment.OutletId,
            newAssignment.CustomerId,
            newAssignment.AssignedAtUtc.UtcDateTime,
            newAssignment.RemovedAtUtc?.UtcDateTime,
            newAssignment.RemovedAtUtc == null));
    }
}
