using CIOT.Common.Domain;

namespace CIOT.Modules.Mobile.Domain.Entities;

public sealed class OfflineBatch : BaseEntity<Guid>, ICreationAuditableEntity
{
    public string BatchId { get; set; }
    public Guid RequestedByUserId { get; set; }
    public string? AppVersion { get; set; }
    public string Status { get; set; }
    public int ActionCount { get; set; }
    public int CompletedCount { get; set; }
    public int RejectedCount { get; set; }
    public int ReplayedCount { get; set; }
    public string CorrelationId { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? CompletedAtUtc { get; set; }
    public DateTimeOffset ResultExpiresAtUtc { get; set; }
}