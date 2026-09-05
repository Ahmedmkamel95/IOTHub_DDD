using CIOT.Common.CQRS;
using CIOT.Common.Results;
using CIOT.Modules.Devices.Application.Dtos;
using CIOT.Modules.Devices.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.Devices.Application.Queries;

public record GetDevicesQuery(string? Status = null, int Page = 1, int PageSize = 20) : IQuery<List<DeviceDto>>;
public record GetDeviceByIdQuery(Guid Id) : IQuery<DeviceDto>;
public record GetDeviceCommandsQuery(Guid DeviceId) : IQuery<List<DeviceCommandDto>>;

public class DeviceQueryHandlers :
    IRequestHandler<GetDevicesQuery, Result<List<DeviceDto>>>,
    IRequestHandler<GetDeviceByIdQuery, Result<DeviceDto>>,
    IRequestHandler<GetDeviceCommandsQuery, Result<List<DeviceCommandDto>>>
{
    private readonly DevicesDbContext _dbContext;

    public DeviceQueryHandlers(DevicesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<DeviceDto>>> Handle(GetDevicesQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Devices.AsNoTracking().Include(d => d.Assignments).AsQueryable();

        if (!string.IsNullOrEmpty(request.Status))
        {
            query = query.Where(d => d.LifecycleStatus.ToLower() == request.Status.ToLower());
        }

        var list = await query
            .OrderByDescending(d => d.CreatedAtUtc)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(d => new DeviceDto(
                d.Id,
                d.IotHubDeviceId,
                d.DeviceSerialNumber,
                d.Imei,
                d.MacAddress,
                d.CountryCode,
                d.LifecycleStatus,
                d.FirmwareVersion,
                d.FirstSeenAtUtc,
                d.LastSeenAtUtc,
                d.Assignments.Where(a => a.UnpairedAtUtc == null).Select(a => (Guid?)a.AssetId).FirstOrDefault()
            ))
            .ToListAsync(cancellationToken);

        return Result.Success(list);
    }

    public async Task<Result<DeviceDto>> Handle(GetDeviceByIdQuery request, CancellationToken cancellationToken)
    {
        var d = await _dbContext.Devices
            .AsNoTracking()
            .Include(x => x.Assignments)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (d == null)
            return Result.Failure<DeviceDto>(Error.NotFound("Device.NotFound", $"Device '{request.Id}' not found."));

        var dto = new DeviceDto(
            d.Id,
            d.IotHubDeviceId,
            d.DeviceSerialNumber,
            d.Imei,
            d.MacAddress,
            d.CountryCode,
            d.LifecycleStatus,
            d.FirmwareVersion,
            d.FirstSeenAtUtc,
            d.LastSeenAtUtc,
            d.Assignments.Where(a => a.UnpairedAtUtc == null).Select(a => (Guid?)a.AssetId).FirstOrDefault()
        );

        return Result.Success(dto);
    }

    public async Task<Result<List<DeviceCommandDto>>> Handle(GetDeviceCommandsQuery request, CancellationToken cancellationToken)
    {
        var list = await _dbContext.DeviceCommands
            .AsNoTracking()
            .Where(c => c.DeviceId == request.DeviceId)
            .OrderByDescending(c => c.CreatedAtUtc)
            .Select(c => new DeviceCommandDto(
                c.Id,
                c.DeviceId,
                c.CommandType,
                c.PayloadJson,
                c.DeliveryPath,
                c.Status,
                c.EnqueuedAtUtc,
                c.CompletedAtUtc,
                c.LastError
            ))
            .ToListAsync(cancellationToken);

        return Result.Success(list);
    }
}
