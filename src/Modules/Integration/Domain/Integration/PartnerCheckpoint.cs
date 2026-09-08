using CIOT.Common.Domain;

namespace CIOT.Modules.Integration.Domain.Entities;

public sealed class PartnerCheckpoint : BaseEntity<Guid>, IUpdateAuditableEntity
{
    public string PartnerCode { get; set; } = null!;
    public string FeedCode { get; set; } = null!;
    public string? CheckpointValue { get; set; }
    public DateTimeOffset? LastSuccessAtUtc { get; set; }
    public DateTimeOffset? LastAttemptAtUtc { get; set; }
    public int ConsecutiveFailureCount { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
}