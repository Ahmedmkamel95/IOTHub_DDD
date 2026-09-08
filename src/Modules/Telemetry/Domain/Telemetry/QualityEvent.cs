using CIOT.Common.Domain;

namespace CIOT.Modules.Telemetry.Domain.Entities;

public sealed class QualityEvent : BaseEntity<Guid>, ICreationAuditableEntity
{
    public DateTimeOffset EventAtUtc { get; set; }

    public Guid? RawMessageId { get; set; }

    public Guid? NormalizedEventId { get; set; }

    public DateTimeOffset? NormalizedEventAtUtc { get; set; }

    public Guid? DeviceId { get; set; }

    public Guid? AssetId { get; set; }

    public Guid? OutletId { get; set; }

    public Guid? CustomerId { get; set; }

    public string? CountryCode { get; set; }

    public string? MachineType { get; set; }

    public string QualityEventCode { get; set; } = default!;

    public string Classification { get; set; } = default!;

    public int? Button { get; set; }

    public string? ButtonCode { get; set; }

    public int? RecipeNumber { get; set; }

    public string? RecipeCode { get; set; }

    public int? GroupNumber { get; set; }

    public string? GroupCode { get; set; }

    public string? RawType { get; set; }

    public int? TypeValue { get; set; }

    public bool? InRange { get; set; }

    public bool? Preinfusion { get; set; }

    public bool? Flush { get; set; }

    public long? DurationMs { get; set; }

    public long? PulseCount { get; set; }

    public decimal? NumericValue { get; set; }

    public string? RawDebugJson { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }
}