using CIOT.Common.Domain;

namespace CIOT.Modules.Telemetry.Domain.Entities;

public sealed class LocalNormalizedMessageQueue : ICreationAuditableEntity
{
    public Guid NormalizedMessageId { get; set; }
    public Guid RawMessageId { get; set; }
    public string PartitionKey { get; set; }
    public string PayloadJson { get; set; }
    public string Status { get; set; }
    public int AttemptCount { get; set; }
    public DateTimeOffset AvailableAtUtc { get; set; }
    public DateTimeOffset? LeaseUntilUtc { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? CompletedAtUtc { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }

    public RawMessage RawMessage { get; set; } = null!;
}