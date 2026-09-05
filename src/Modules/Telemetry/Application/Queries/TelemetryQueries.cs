using CIOT.Common.CQRS;
using CIOT.Common.Results;
using CIOT.Modules.Telemetry.Application.Dtos;
using CIOT.Modules.Telemetry.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.Telemetry.Application.Queries;

public record GetLatestAssetStateQuery(Guid AssetId) : IQuery<AssetCurrentStateDto>;
public record GetMeasurementsQuery(Guid? DeviceId = null, Guid? AssetId = null, string? MetricKey = null, int Limit = 50) : IQuery<List<MeasurementDto>>;

public class TelemetryQueryHandlers :
    IRequestHandler<GetLatestAssetStateQuery, Result<AssetCurrentStateDto>>,
    IRequestHandler<GetMeasurementsQuery, Result<List<MeasurementDto>>>
{
    private readonly TelemetryDbContext _dbContext;

    public TelemetryQueryHandlers(TelemetryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<AssetCurrentStateDto>> Handle(GetLatestAssetStateQuery request, CancellationToken cancellationToken)
    {
        var state = await _dbContext.AssetCurrentStates.AsNoTracking()
            .FirstOrDefaultAsync(s => s.AssetId == request.AssetId, cancellationToken);

        if (state == null)
            return Result.Failure<AssetCurrentStateDto>(Error.NotFound("State.NotFound", $"No current state found for asset '{request.AssetId}'."));

        var dto = new AssetCurrentStateDto(
            state.AssetId,
            state.DeviceId,
            state.LastTelemetryAtUtc,
            state.MachineStatus,
            state.WaterLitersToday,
            state.EnergyKwhToday,
            state.CoffeeKgToday,
            state.CupsToday,
            state.Latitude,
            state.Longitude
        );

        return Result.Success(dto);
    }

    public async Task<Result<List<MeasurementDto>>> Handle(GetMeasurementsQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.NormalizedMeasurements.AsNoTracking().AsQueryable();

        if (request.DeviceId.HasValue) query = query.Where(m => m.DeviceId == request.DeviceId);
        if (request.AssetId.HasValue) query = query.Where(m => m.AssetId == request.AssetId);
        if (!string.IsNullOrEmpty(request.MetricKey)) query = query.Where(m => m.MetricKey == request.MetricKey);

        var list = await query
            .OrderByDescending(m => m.MeasuredAtUtc)
            .Take(request.Limit)
            .Select(m => new MeasurementDto(
                m.Id,
                m.DeviceId,
                m.AssetId,
                m.MeasuredAtUtc,
                m.MetricKey,
                m.NumericValue,
                m.UnitOfMeasure
            ))
            .ToListAsync(cancellationToken);

        return Result.Success(list);
    }
}
