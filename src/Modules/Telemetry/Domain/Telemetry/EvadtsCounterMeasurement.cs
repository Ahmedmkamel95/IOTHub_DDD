using CIOT.Common.Domain;

namespace CIOT.Modules.Telemetry.Domain.Entities;

public sealed class EvadtsCounterMeasurement : BaseEntity<Guid>, ICreationAuditableEntity
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

    public string CounterCode { get; set; } = default!;

    public string? CounterGroup { get; set; }

    public string CounterKind { get; set; } = default!;

    public decimal? TotalValue { get; set; }

    public decimal? DeltaValue { get; set; }

    public decimal? PreviousTotalValue { get; set; }

    public DateTimeOffset? PreviousMeasuredAtUtc { get; set; }

    public long? MonetaryAmountMinor { get; set; }

    public string? CurrencyCode { get; set; }

    public string? BillingPeriod { get; set; }

    public string? SourceEventHash { get; set; }

    public string? DimensionsJson { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }
}