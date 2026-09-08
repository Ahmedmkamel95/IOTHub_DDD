using CIOT.Common.Domain;

namespace CIOT.Modules.Mobile.Domain.Entities;

public sealed class OfflineActionResult : BaseEntity<Guid>
{
    public Guid FirstBatchId { get; set; }
    public Guid RequestedByUserId { get; set; }
    public string ClientActionId { get; set; }
    public string ActionType { get; set; }
    public DateTimeOffset? ObservedAtUtc { get; set; }
    public string PayloadHash { get; set; }
    public string Status { get; set; }
    public string? OutcomeEntityType { get; set; }
    public Guid? OutcomeEntityId { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorField { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTimeOffset ProcessedAtUtc { get; set; }
}