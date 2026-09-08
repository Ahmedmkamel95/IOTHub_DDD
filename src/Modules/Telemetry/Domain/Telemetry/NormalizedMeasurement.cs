using CIOT.Common.Domain;

namespace CIOT.Modules.Telemetry.Domain.Entities;

public sealed class NormalizedMeasurement : BaseEntity<Guid>, ICreationAuditableEntity
{
    public DateTimeOffset ObservedAtUtc { get; set; }

    public Guid? RawMessageId { get; set; }
    public Guid? SourceNormalizedEventId { get; set; }
    public DateTimeOffset? SourceNormalizedEventAtUtc { get; set; }

    public bool IsDerived { get; set; }
    public string? DerivationMethod { get; set; }
    public string? DerivationVersion { get; set; }
    public string? DerivationPayloadJson { get; set; }

    public Guid? DeviceId { get; set; }
    public Guid? AssetId { get; set; }
    public Guid? OutletId { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CountryCode { get; set; }
    public string? MachineType { get; set; }

    public string MetricKey { get; set; } = default!;
    public string? Unit { get; set; }
    public decimal? ValueNumeric { get; set; }
    public string? ValueText { get; set; }
    public bool? ValueBool { get; set; }
    public string? ValueJson { get; set; }

    public DateTimeOffset? WindowStartUtc { get; set; }
    public DateTimeOffset? WindowEndUtc { get; set; }
    public string? DimensionsJson { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }

    public RawMessage? RawMessage { get; set; }
}