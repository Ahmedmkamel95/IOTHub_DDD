namespace CIOT.Modules.Telemetry.Application.Dtos;

public record MetricValueDto(string MetricKey, double Value, string? Unit = null);

public record TelemetryIngestRequest(
    Guid? DeviceId,
    Guid? AssetId,
    DateTime? TimestampUtc,
    List<MetricValueDto> Metrics,
    string? MachineStatus = null,
    decimal? Latitude = null,
    decimal? Longitude = null
);

public record MeasurementDto(
    Guid Id,
    Guid? DeviceId,
    Guid? AssetId,
    DateTime MeasuredAtUtc,
    string MetricKey,
    double NumericValue,
    string? UnitOfMeasure
);

public record AssetCurrentStateDto(
    Guid AssetId,
    Guid? DeviceId,
    DateTime LastTelemetryAtUtc,
    string? MachineStatus,
    decimal? WaterLitersToday,
    decimal? EnergyKwhToday,
    decimal? CoffeeKgToday,
    int? CupsToday,
    decimal? Latitude,
    decimal? Longitude
);

public record RawMessageDto(Guid Id, string MessageId, string Status, DateTime ReceivedAtUtc);
