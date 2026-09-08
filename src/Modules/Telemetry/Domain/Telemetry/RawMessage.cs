using CIOT.Common.Domain;

namespace CIOT.Modules.Telemetry.Domain.Entities;

public sealed class RawMessage : BaseEntity<Guid>, ICreationActorAuditableEntity, IUpdateActorAuditableEntity, IOptimisticConcurrentEntity
{
    public string SourceType { get; set; } = default!;
    public string SourceName { get; set; } = default!;
    public string? SourceMessageId { get; set; }
    public Guid? DeviceId { get; set; }

    public DateTimeOffset ReceivedAtUtc { get; set; }

    public string? SchemaName { get; set; }
    public int? SchemaVersion { get; set; }
    public int? DictionaryVersion { get; set; }
    public string? ContentType { get; set; }
    public string? PayloadHash { get; set; }
    public string? RawArchiveUri { get; set; }
    public string? OriginalPayloadText { get; set; }
    public string? OriginalPayloadBase64 { get; set; }
    public string? ConvertedPayloadJson { get; set; }
    public string DecodeStatus { get; set; } = default!;
    public string? DecodeError { get; set; }
    public DateTimeOffset? RetentionUntilUtc { get; set; }

    public string? SourcePartitionId { get; set; }
    public string? SourceOffset { get; set; }
    public long? SourceSequenceNumber { get; set; }
    public DateTimeOffset? SourceEnqueuedAtUtc { get; set; }
    public string? SystemPropertiesJson { get; set; }
    public string? ApplicationPropertiesJson { get; set; }
    public string? DecodeErrorCode { get; set; }
    public int? NormalizedRecordCount { get; set; }
    public DateTimeOffset? NormalizedPublishedAtUtc { get; set; }
    public Guid? PartnerSourceId { get; set; }

    public string? SourceSystem { get; set; }
    public string? SourceRecordId { get; set; }
    public string MessageId { get; set; } = default!;
    public string? DeviceIdentifier { get; set; }
    public string? AssetIdentifier { get; set; }
    public DateTimeOffset? SentAtUtc { get; set; }
    public string PayloadJson { get; set; } = default!;
    public string Status { get; set; } = default!;
    public string? CorrelationId { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }
    public Guid? CreatedByUserId { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
    public Guid? UpdatedByUserId { get; set; }
    public long RowVersion { get; set; }
    public ICollection<LocalNormalizedMessageQueue> LocalNormalizedMessageQueues { get; } =
        new List<LocalNormalizedMessageQueue>();
}