using CIOT.Common.Domain;

namespace CIOT.Modules.Integration.Domain.Entities;

public sealed class PartnerRawOutbox : BaseEntity<Guid>, ICreationAuditableEntity, IUpdateAuditableEntity
{
    public Guid IngestionRunId { get; set; }
    public int RunItemIndex { get; set; }
    public int RunItemCount { get; set; }
    public Guid PartnerSourceId { get; set; }
    public string PartnerCode { get; set; } = null!;
    public string FeedCode { get; set; } = null!;
    public string SourceRecordId { get; set; } = null!;
    public string SourceEntityKey { get; set; } = null!;
    public string? CheckpointValue { get; set; }
    public string PackageJson { get; set; } = null!;
    public string PayloadHash { get; set; } = null!;
    public string Status { get; set; } = null!;
    public int PublishAttemptCount { get; set; }
    public DateTimeOffset NextAttemptAtUtc { get; set; }
    public string? LockedBy { get; set; }
    public DateTimeOffset? LockedUntilUtc { get; set; }
    public DateTimeOffset? PublishedAtUtc { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
}