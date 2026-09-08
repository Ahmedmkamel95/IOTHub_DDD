using CIOT.Common.Domain;

namespace CIOT.Modules.Devices.Domain.Entities;

public sealed class FirmwareAssignmentHandoffOutbox : BaseEntity<Guid>, ICreationAuditableEntity
{
    public Guid AssignmentId { get; set; }
    public string EventType { get; set; }
    public short SchemaVersion { get; set; }
    public string PayloadJson { get; set; }
    public string Status { get; set; }
    public short AttemptCount { get; set; }
    public DateTimeOffset NextAttemptAtUtc { get; set; }
    public string? ErrorMessage { get; set; }
    public string CorrelationId { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? ProcessedAtUtc { get; set; }
}