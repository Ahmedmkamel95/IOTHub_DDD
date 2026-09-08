using CIOT.Common.Domain;

namespace CIOT.Modules.Audit.Domain.Entities;

public sealed class AuditEvent : BaseEntity<Guid>
{
    public DateTimeOffset EventAtUtc { get; set; }

    public Guid? ActorUserId { get; set; }

    public Guid? ActorApiClientId { get; set; }

    public string ActorType { get; set; } = "system";

    public string Action { get; set; } = default!;

    public string? EntityType { get; set; }

    public Guid? EntityId { get; set; }

    public string? CountryCode { get; set; }

    public string? CorrelationId { get; set; }

    public string? RequestId { get; set; }

    public string? BeforeJson { get; set; }

    public string? AfterJson { get; set; }

    public string? MetadataJson { get; set; }
}