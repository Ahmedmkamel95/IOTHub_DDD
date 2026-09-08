using CIOT.Common.Domain;

namespace CIOT.Modules.Telemetry.Domain.Entities;

public sealed class WaterConsumptionMeasurement : BaseEntity<Guid>, ICreationAuditableEntity
{
    public DateTimeOffset MeasuredAtUtc { get; set; }

    public Guid? RawMessageId { get; set; }

    public Guid? NormalizedMeasurementId { get; set; }

    public DateTimeOffset? NormalizedObservedAtUtc { get; set; }

    public Guid? DeviceId { get; set; }

    public Guid? AssetId { get; set; }

    public Guid? OutletId { get; set; }

    public Guid? CustomerId { get; set; }

    public string? CountryCode { get; set; }

    public string? MachineType { get; set; }

    public decimal WaterL { get; set; }

    public decimal? FilterLifetimeL { get; set; }

    public decimal? FilterRemainingL { get; set; }

    public string? FilterStatus { get; set; }

    public DateTimeOffset? PeriodStartUtc { get; set; }

    public DateTimeOffset? PeriodEndUtc { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }
}