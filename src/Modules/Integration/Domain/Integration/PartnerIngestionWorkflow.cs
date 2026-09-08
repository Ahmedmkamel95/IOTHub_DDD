using CIOT.Common.Domain;

namespace CIOT.Modules.Integration.Domain.Entities;

public sealed class PartnerIngestionWorkflow : BaseEntity<Guid>, ICreationActorAuditableEntity, IUpdateActorAuditableEntity, IOptimisticConcurrentEntity
{
    public Guid PartnerSourceId { get; set; }
    public string PartnerCode { get; set; } = default!;
    public string FeedCode { get; set; } = default!;

    public string? SourceSystem { get; set; }
    public string? SourceBatchId { get; set; }
    public string? SourceRecordId { get; set; }
    public string WorkflowType { get; set; } = default!;
    public string WorkflowStatus { get; set; } = default!;
    public string State { get; set; } = default!;

    public string? CheckpointBefore { get; set; }
    public string? CheckpointAfter { get; set; }
    public DateTimeOffset? SourceWindowStartAtUtc { get; set; }
    public DateTimeOffset? SourceWindowEndAtUtc { get; set; }
    public string? RemoteWorkflowReference { get; set; }
    public string? RemoteTokenSecretReference { get; set; }
    public string? RawArchiveUri { get; set; }
    public string? RawArchiveSha256 { get; set; }

    public int RetrievedCount { get; set; }
    public int StagedCount { get; set; }
    public int PublishedCount { get; set; }
    public int ProcessedCount { get; set; }

    public string? CorrelationId { get; set; }
    public string? PayloadHash { get; set; }

    public int AttemptCount { get; set; }
    public int RetryCount { get; set; }
    public DateTimeOffset? NextAttemptAtUtc { get; set; }
    public DateTimeOffset? NextRetryAtUtc { get; set; }
    public string? LeaseOwner { get; set; }
    public DateTimeOffset? LeaseUntilUtc { get; set; }

    public DateTimeOffset StartedAtUtc { get; set; }
    public DateTimeOffset? ReportReadyAtUtc { get; set; }
    public DateTimeOffset? FileSavedAtUtc { get; set; }
    public DateTimeOffset? StagedAtUtc { get; set; }
    public DateTimeOffset? PublishedAtUtc { get; set; }
    public DateTimeOffset? CompletedAtUtc { get; set; }

    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public string? StateJson { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }
    public Guid? CreatedByUserId { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
    public Guid? UpdatedByUserId { get; set; }
    public long RowVersion { get; set; }
}