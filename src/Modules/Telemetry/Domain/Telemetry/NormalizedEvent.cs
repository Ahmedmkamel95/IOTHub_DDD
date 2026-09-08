using CIOT.Common.Domain;

namespace CIOT.Modules.Telemetry.Domain.Entities;

public sealed class NormalizedEvent : BaseEntity<Guid>, ICreationAuditableEntity
{
    public DateTimeOffset EventAtUtc { get; set; }

    public Guid? RawMessageId { get; set; }
    public Guid? DeviceId { get; set; }
    public Guid? AssetId { get; set; }
    public Guid? OutletId { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CountryCode { get; set; }
    public string? MachineType { get; set; }

    public string EventType { get; set; } = default!;
    public string EventCode { get; set; } = default!;
    public string? Severity { get; set; }
    public string? State { get; set; }
    public string? SourceCode { get; set; }
    public string? EventValue { get; set; }
    public string? EventPayloadJson { get; set; }

    public string? CorrelationId { get; set; }
    public string? MetadataJson { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }

    public RawMessage? RawMessage { get; set; }
}