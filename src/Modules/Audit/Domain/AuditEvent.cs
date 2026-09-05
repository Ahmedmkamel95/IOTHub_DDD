using CIOT.Common.Domain;

namespace CIOT.Modules.Audit.Domain;

public sealed class AuditEvent : AggregateRoot
{
    public Guid? UserId { get; set; }
    public string Action { get; set; } = null!; // "Create", "Update", "Delete", "CommandDispatched"
    public string EntityType { get; set; } = null!;
    public string EntityId { get; set; } = null!;
    public string? ChangesJson { get; set; }
    public string? IpAddress { get; set; }
}

