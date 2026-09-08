using CIOT.Common.Domain;

namespace CIOT.Modules.Integration.Domain.Entities;

public sealed class PartnerSourceCheckpoint : ICreationActorAuditableEntity, IUpdateActorAuditableEntity, IOptimisticConcurrentEntity
{
    public Guid PartnerSourceId { get; set; }
    public string PartnerSourceCode { get; set; } = default!;
    public string FeedCode { get; set; } = default!;

    public string? CheckpointValue { get; set; }
    public string? LastProcessedRecordId { get; set; }
    public DateTimeOffset? LastProcessedAtUtc { get; set; }
    public DateTimeOffset? LastSuccessAtUtc { get; set; }
    public DateTimeOffset? LastAttemptAtUtc { get; set; }
    public int ConsecutiveFailureCount { get; set; }

    public string? LeaseOwner { get; set; }
    public DateTimeOffset? LeaseExpiresAtUtc { get; set; }
    public DateTimeOffset? LeaseUntilUtc { get; set; }

    public string Status { get; set; } = default!;
    public string? MetadataJson { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }
    public Guid? CreatedByUserId { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
    public Guid? UpdatedByUserId { get; set; }
    public long RowVersion { get; set; }
}