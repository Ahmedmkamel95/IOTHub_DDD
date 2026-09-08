using CIOT.Common.Domain;

namespace CIOT.Modules.Telemetry.Domain.Entities;

public sealed class VendEvent : BaseEntity<Guid>, ICreationAuditableEntity
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

    public string SourceType { get; set; } = default!;

    public string? SourceEventId { get; set; }

    public string? ProductCode { get; set; }

    public Guid? MaterialId { get; set; }

    public decimal? VendCount { get; set; }

    public decimal? Quantity { get; set; }

    public string? Unit { get; set; }

    public long? AmountMinor { get; set; }

    public string? CurrencyCode { get; set; }

    public string? PaymentType { get; set; }

    public string? EventPayloadJson { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }
}