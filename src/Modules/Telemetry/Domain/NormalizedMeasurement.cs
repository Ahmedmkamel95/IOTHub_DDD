namespace CIOT.Modules.Telemetry.Domain;

public sealed class NormalizedMeasurement
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime MeasuredAtUtc { get; set; }
    public Guid? DeviceId { get; set; }
    public Guid? AssetId { get; set; }
    public Guid? OutletId { get; set; }
    public string? CountryCode { get; set; }
    public string MetricKey { get; set; } = default!; // e.g. "temperature", "pressure", "water_flow", "cups"
    public double NumericValue { get; set; }
    public string? UnitOfMeasure { get; set; }
    public bool IsDerived { get; set; }
    public Guid? RawMessageId { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
