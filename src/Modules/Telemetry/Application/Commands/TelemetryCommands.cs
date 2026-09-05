using CIOT.Common.CQRS;
using CIOT.Common.Results;
using CIOT.Modules.Telemetry.Application.Dtos;
using CIOT.Modules.Telemetry.Domain;
using CIOT.Modules.Telemetry.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.Telemetry.Application.Commands;

public record IngestTelemetryCommand(TelemetryIngestRequest Request) : ICommand<int>;
public record IngestRawMessageCommand(string PayloadJson, string? DeviceIdentifier = null, string? AssetIdentifier = null) : ICommand<RawMessageDto>;

public class IngestTelemetryCommandValidator : AbstractValidator<IngestTelemetryCommand>
{
    public IngestTelemetryCommandValidator()
    {
        RuleFor(x => x.Request.Metrics).NotEmpty().WithMessage("At least one metric is required.");
    }
}

public class TelemetryCommandHandlers :
    IRequestHandler<IngestTelemetryCommand, Result<int>>,
    IRequestHandler<IngestRawMessageCommand, Result<RawMessageDto>>
{
    private readonly TelemetryDbContext _dbContext;

    public TelemetryCommandHandlers(TelemetryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<int>> Handle(IngestTelemetryCommand command, CancellationToken cancellationToken)
    {
        var req = command.Request;
        var measuredAt = req.TimestampUtc ?? DateTime.UtcNow;

        var measurements = req.Metrics.Select(m => new NormalizedMeasurement
        {
            DeviceId = req.DeviceId,
            AssetId = req.AssetId,
            MeasuredAtUtc = measuredAt,
            MetricKey = m.MetricKey,
            NumericValue = m.Value,
            UnitOfMeasure = m.Unit,
            CreatedAtUtc = DateTime.UtcNow
        }).ToList();

        _dbContext.NormalizedMeasurements.AddRange(measurements);

        // Update AssetCurrentState if AssetId is provided
        if (req.AssetId.HasValue)
        {
            var state = await _dbContext.AssetCurrentStates
                .FirstOrDefaultAsync(s => s.AssetId == req.AssetId.Value, cancellationToken);

            if (state == null)
            {
                state = new AssetCurrentState
                {
                    AssetId = req.AssetId.Value,
                    DeviceId = req.DeviceId,
                    LastTelemetryAtUtc = measuredAt,
                    MachineStatus = req.MachineStatus,
                    Latitude = req.Latitude,
                    Longitude = req.Longitude
                };
                _dbContext.AssetCurrentStates.Add(state);
            }
            else
            {
                state.LastTelemetryAtUtc = measuredAt;
                if (req.DeviceId.HasValue) state.DeviceId = req.DeviceId;
                if (!string.IsNullOrEmpty(req.MachineStatus)) state.MachineStatus = req.MachineStatus;
                if (req.Latitude.HasValue) state.Latitude = req.Latitude;
                if (req.Longitude.HasValue) state.Longitude = req.Longitude;
            }

            // Extract well-known metrics into state columns
            foreach (var m in req.Metrics)
            {
                switch (m.MetricKey.ToLowerInvariant())
                {
                    case "cups": state.CupsToday = (int)m.Value; break;
                    case "water_liters": state.WaterLitersToday = (decimal)m.Value; break;
                    case "energy_kwh": state.EnergyKwhToday = (decimal)m.Value; break;
                    case "coffee_kg": state.CoffeeKgToday = (decimal)m.Value; break;
                }
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success(measurements.Count);
    }

    public async Task<Result<RawMessageDto>> Handle(IngestRawMessageCommand command, CancellationToken cancellationToken)
    {
        var raw = new RawMessage
        {
            PayloadJson = command.PayloadJson,
            DeviceIdentifier = command.DeviceIdentifier,
            AssetIdentifier = command.AssetIdentifier,
            Status = "Received",
            ReceivedAtUtc = DateTime.UtcNow
        };

        _dbContext.RawMessages.Add(raw);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new RawMessageDto(raw.Id, raw.MessageId, raw.Status, raw.ReceivedAtUtc));
    }
}
