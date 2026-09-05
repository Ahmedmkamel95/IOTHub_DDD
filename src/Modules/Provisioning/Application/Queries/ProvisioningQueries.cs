using CIOT.Common.CQRS;
using CIOT.Common.Results;
using CIOT.Modules.Provisioning.Application.Dtos;
using CIOT.Modules.Provisioning.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.Provisioning.Application.Queries;

public record GetDeviceManufacturersQuery : IQuery<List<DeviceManufacturerDto>>;
public record GetDeviceModelsQuery(Guid? ManufacturerId = null) : IQuery<List<DeviceModelDto>>;

public class ProvisioningQueryHandlers :
    IRequestHandler<GetDeviceManufacturersQuery, Result<List<DeviceManufacturerDto>>>,
    IRequestHandler<GetDeviceModelsQuery, Result<List<DeviceModelDto>>>
{
    private readonly ProvisioningDbContext _dbContext;

    public ProvisioningQueryHandlers(ProvisioningDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<DeviceManufacturerDto>>> Handle(GetDeviceManufacturersQuery request, CancellationToken cancellationToken)
    {
        var list = await _dbContext.DeviceManufacturers.AsNoTracking()
            .OrderBy(m => m.ManufacturerCode)
            .Select(m => new DeviceManufacturerDto(m.Id, m.ManufacturerCode, m.DisplayName, m.Status))
            .ToListAsync(cancellationToken);

        return Result.Success(list);
    }

    public async Task<Result<List<DeviceModelDto>>> Handle(GetDeviceModelsQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.DeviceModels.AsNoTracking().AsQueryable();
        if (request.ManufacturerId.HasValue) query = query.Where(m => m.DeviceManufacturerId == request.ManufacturerId.Value);

        var list = await query
            .OrderBy(m => m.ModelCode)
            .Select(m => new DeviceModelDto(m.Id, m.DeviceManufacturerId, m.ModelCode, m.DisplayName, m.HardwareRevision, m.Status))
            .ToListAsync(cancellationToken);

        return Result.Success(list);
    }
}
