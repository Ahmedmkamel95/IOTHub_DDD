using CIOT.Common.Domain;

namespace CIOT.Modules.Devices.Domain.Entities;

public sealed class DeviceBulkAction : BaseEntity<Guid>
{
    public string ActionType { get; set; }
    public string SelectionJson { get; set; }
    public string ReasonCode { get; set; }
    public string? Reason { get; set; }
    public bool? RecoveryAllowed { get; set; }
    public int TargetCount { get; set; }
    public string Status { get; set; }
    public Guid? RequestedByUserId { get; set; }
    public DateTimeOffset RequestedAtUtc { get; set; }
    public DateTimeOffset? CompletedAtUtc { get; set; }
    public string? CorrelationId { get; set; }
}