using CIOT.Common.Domain;

namespace CIOT.Modules.Integration.Domain.Entities;

public sealed class ImportBatch : BaseEntity<Guid>, ICreationAuditableEntity, IUpdateAuditableEntity
{
    public string? SourceSystem { get; set; } = null!;
    public string? SourceBatchId { get; set; }
    public string CorrelationId { get; set; } = null!;
    public string EndpointName { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTimeOffset ReceivedAtUtc { get; set; }
    public DateTimeOffset? StartedAtUtc { get; set; }
    public DateTimeOffset? CompletedAtUtc { get; set; }
    public int ItemCount { get; set; }
    public int AcceptedCount { get; set; }
    public int RejectedCount { get; set; }
    public string? ResultJson { get; set; }
    public string? PayloadHash { get; set; }
    public string? RequestMetadataJson { get; set; }
    public string? IdempotencyKey { get; set; }
    public string? InputBlobName { get; set; }
    public string? InputFileName { get; set; }
    public string? InputContentType { get; set; }
    public DateTimeOffset? NextAttemptAtUtc { get; set; }
    public string? LeaseOwner { get; set; }
    public DateTimeOffset? LeaseUntilUtc { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTimeOffset? CancelledAtUtc { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
}