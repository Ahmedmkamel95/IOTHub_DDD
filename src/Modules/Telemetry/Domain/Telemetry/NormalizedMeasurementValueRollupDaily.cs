namespace CIOT.Modules.Telemetry.Domain.Entities;

public sealed class NormalizedMeasurementValueRollupDaily
{
    public DateTimeOffset? BucketAtUtc { get; set; }
    public string? MetricKey { get; set; }
    public string? CountryCode { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid? OutletId { get; set; }
    public Guid? AssetId { get; set; }
    public Guid? DeviceId { get; set; }
    public string? Unit { get; set; }
    public long? SourceCount { get; set; }
    public decimal? SumValue { get; set; }
    public decimal? AvgValue { get; set; }
    public decimal? MinValue { get; set; }
    public decimal? MaxValue { get; set; }
}