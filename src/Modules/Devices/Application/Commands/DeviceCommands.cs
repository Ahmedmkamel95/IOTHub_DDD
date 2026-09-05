using CIOT.Common.CQRS;
using CIOT.Common.Results;
using CIOT.Modules.Devices.Application.Dtos;
using CIOT.Modules.Devices.Domain;
using CIOT.Modules.Devices.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.Devices.Application.Commands;

public record RegisterDeviceCommand(RegisterDeviceRequest Request) : ICommand<DeviceDto>;
public record UpdateDeviceStatusCommand(Guid DeviceId, string LifecycleStatus) : ICommand;
public record DispatchDeviceCommandCommand(Guid DeviceId, DispatchCommandRequest Request) : ICommand<DeviceCommandDto>;
public record PairDeviceWithAssetCommand(Guid DeviceId, PairDeviceRequest Request) : ICommand;
public record AcknowledgeDeviceCommandCommand(Guid DeviceId, Guid CommandId, bool Success, string? ErrorMessage = null) : ICommand;

public class RegisterDeviceCommandValidator : AbstractValidator<RegisterDeviceCommand>
{
    public RegisterDeviceCommandValidator()
    {
        RuleFor(x => x.Request.IotHubDeviceId).NotEmpty().MaximumLength(128);
    }
}

public class DeviceCommandHandlers :
    IRequestHandler<RegisterDeviceCommand, Result<DeviceDto>>,
    IRequestHandler<UpdateDeviceStatusCommand, Result>,
    IRequestHandler<DispatchDeviceCommandCommand, Result<DeviceCommandDto>>,
    IRequestHandler<PairDeviceWithAssetCommand, Result>,
    IRequestHandler<AcknowledgeDeviceCommandCommand, Result>
{
    private readonly DevicesDbContext _dbContext;

    public DeviceCommandHandlers(DevicesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<DeviceDto>> Handle(RegisterDeviceCommand command, CancellationToken cancellationToken)
    {
        var req = command.Request;
        var existing = await _dbContext.Devices.AnyAsync(d => d.IotHubDeviceId == req.IotHubDeviceId, cancellationToken);
        if (existing)
        {
            return Result.Failure<DeviceDto>(Error.Conflict("Device.Duplicate", $"Device '{req.IotHubDeviceId}' already registered."));
        }

        var device = new Device
        {
            IotHubDeviceId = req.IotHubDeviceId,
            DeviceSerialNumber = req.DeviceSerialNumber,
            Imei = req.Imei,
            MacAddress = req.MacAddress,
            CountryCode = req.CountryCode?.ToUpperInvariant(),
            FirmwareVersion = req.FirmwareVersion,
            LifecycleStatus = "Registered"
        };

        _dbContext.Devices.Add(device);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new DeviceDto(
            device.Id,
            device.IotHubDeviceId,
            device.DeviceSerialNumber,
            device.Imei,
            device.MacAddress,
            device.CountryCode,
            device.LifecycleStatus,
            device.FirmwareVersion,
            device.FirstSeenAtUtc,
            device.LastSeenAtUtc,
            null
        ));
    }

    public async Task<Result> Handle(UpdateDeviceStatusCommand command, CancellationToken cancellationToken)
    {
        var device = await _dbContext.Devices.FirstOrDefaultAsync(d => d.Id == command.DeviceId, cancellationToken);
        if (device == null)
            return Result.Failure(Error.NotFound("Device.NotFound", $"Device '{command.DeviceId}' not found."));

        switch (command.LifecycleStatus.ToLowerInvariant())
        {
            case "active": device.Activate(); break;
            case "suspended": device.Suspend(); break;
            case "decommissioned": device.Decommission(); break;
            default: device.LifecycleStatus = command.LifecycleStatus; break;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result<DeviceCommandDto>> Handle(DispatchDeviceCommandCommand command, CancellationToken cancellationToken)
    {
        var device = await _dbContext.Devices.FirstOrDefaultAsync(d => d.Id == command.DeviceId, cancellationToken);
        if (device == null)
            return Result.Failure<DeviceCommandDto>(Error.NotFound("Device.NotFound", $"Device '{command.DeviceId}' not found."));

        var cmd = new DeviceCommand
        {
            DeviceId = device.Id,
            CommandType = command.Request.CommandType,
            PayloadJson = command.Request.PayloadJson,
            DeliveryPath = command.Request.DeliveryPath,
            Status = "Enqueued",
            EnqueuedAtUtc = DateTime.UtcNow
        };

        _dbContext.DeviceCommands.Add(cmd);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new DeviceCommandDto(
            cmd.Id,
            cmd.DeviceId,
            cmd.CommandType,
            cmd.PayloadJson,
            cmd.DeliveryPath,
            cmd.Status,
            cmd.EnqueuedAtUtc,
            cmd.CompletedAtUtc,
            cmd.LastError
        ));
    }

    public async Task<Result> Handle(PairDeviceWithAssetCommand command, CancellationToken cancellationToken)
    {
        var device = await _dbContext.Devices
            .Include(d => d.Assignments)
            .FirstOrDefaultAsync(d => d.Id == command.DeviceId, cancellationToken);

        if (device == null)
            return Result.Failure(Error.NotFound("Device.NotFound", $"Device '{command.DeviceId}' not found."));

        // Unpair previous active assignments
        foreach (var assignment in device.Assignments.Where(a => a.IsActive))
        {
            assignment.Unpair();
        }

        var newAssignment = new DeviceAssignment
        {
            DeviceId = device.Id,
            AssetId = command.Request.AssetId,
            AssignmentType = command.Request.AssignmentType,
            PairedAtUtc = DateTime.UtcNow
        };

        device.Assignments.Add(newAssignment);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> Handle(AcknowledgeDeviceCommandCommand command, CancellationToken cancellationToken)
    {
        var cmd = await _dbContext.DeviceCommands
            .FirstOrDefaultAsync(c => c.Id == command.CommandId && c.DeviceId == command.DeviceId, cancellationToken);

        if (cmd == null)
            return Result.Failure(Error.NotFound("Command.NotFound", $"Command '{command.CommandId}' not found for device '{command.DeviceId}'."));

        if (command.Success)
        {
            cmd.MarkCompleted();
        }
        else
        {
            cmd.MarkFailed(command.ErrorMessage ?? "Device returned execution failure.");
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
