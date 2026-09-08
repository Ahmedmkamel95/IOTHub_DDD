using CIOT.Common.Domain;

namespace CIOT.Modules.Telemetry.Domain.Entities;

public sealed class PersistenceDeadLetter : BaseEntity<Guid>, ICreationAuditableEntity
{
    public DateTimeOffset ReceivedAtUtc { get; set; }
    public string? SourceEventhubName { get; set; }
    public string? ConsumerGroup { get; set; }
    public string? PartitionId { get; set; }
    public long? SequenceNumber { get; set; }
    public string? OffsetText { get; set; }
    public Guid? RawMessageId { get; set; }
    public Guid? NormalizedMessageId { get; set; }
    public Guid? DeviceId { get; set; }
    public string ErrorCode { get; set; } = default!;
    public string? ErrorMessage { get; set; }
    public string PayloadJson { get; set; } = default!;
    public bool Retryable { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
}