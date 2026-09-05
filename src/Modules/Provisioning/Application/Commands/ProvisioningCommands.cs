using CIOT.Common.CQRS;
using CIOT.Common.Results;
using CIOT.Modules.Provisioning.Application.Dtos;
using CIOT.Modules.Provisioning.Domain;
using CIOT.Modules.Provisioning.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.Provisioning.Application.Commands;

public record CreateDeviceManufacturerCommand(CreateDeviceManufacturerRequest Request) : ICommand<DeviceManufacturerDto>;
public record CreateDeviceModelCommand(CreateDeviceModelRequest Request) : ICommand<DeviceModelDto>;

public class ProvisioningCommandHandlers :
    IRequestHandler<CreateDeviceManufacturerCommand, Result<DeviceManufacturerDto>>,
    IRequestHandler<CreateDeviceModelCommand, Result<DeviceModelDto>>
{
    private readonly ProvisioningDbContext _dbContext;

    public ProvisioningCommandHandlers(ProvisioningDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<DeviceManufacturerDto>> Handle(CreateDeviceManufacturerCommand command, CancellationToken cancellationToken)
    {
        var req = command.Request;
        var existing = await _dbContext.DeviceManufacturers.AnyAsync(m => m.ManufacturerCode.ToUpper() == req.ManufacturerCode.ToUpper(), cancellationToken);
        if (existing)
        {
            return Result.Failure<DeviceManufacturerDto>(Error.Conflict("Manufacturer.Duplicate", $"Manufacturer '{req.ManufacturerCode}' already exists."));
        }

        var m = new DeviceManufacturer
        {
            ManufacturerCode = req.ManufacturerCode.ToUpperInvariant(),
            DisplayName = req.DisplayName,
            Status = "Active"
        };

        _dbContext.DeviceManufacturers.Add(m);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new DeviceManufacturerDto(m.Id, m.ManufacturerCode, m.DisplayName, m.Status));
    }

    public async Task<Result<DeviceModelDto>> Handle(CreateDeviceModelCommand command, CancellationToken cancellationToken)
    {
        var req = command.Request;
        var manufacturerExists = await _dbContext.DeviceManufacturers.AnyAsync(m => m.Id == req.DeviceManufacturerId, cancellationToken);
        if (!manufacturerExists)
        {
            return Result.Failure<DeviceModelDto>(Error.NotFound("Manufacturer.NotFound", $"Manufacturer '{req.DeviceManufacturerId}' not found."));
        }

        var model = new DeviceModel
        {
            DeviceManufacturerId = req.DeviceManufacturerId,
            ModelCode = req.ModelCode.ToUpperInvariant(),
            DisplayName = req.DisplayName,
            HardwareRevision = req.HardwareRevision,
            Status = "Active"
        };

        _dbContext.DeviceModels.Add(model);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new DeviceModelDto(model.Id, model.DeviceManufacturerId, model.ModelCode, model.DisplayName, model.HardwareRevision, model.Status));
    }
}
