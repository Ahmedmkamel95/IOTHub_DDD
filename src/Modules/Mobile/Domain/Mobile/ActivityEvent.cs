using CIOT.Common.Domain;

namespace CIOT.Modules.Mobile.Domain.Entities;

public sealed class ActivityEvent : BaseEntity<Guid>
{
    public Guid UserId { get; set; }
    public string ActionType { get; set; }
    public Guid? AssetId { get; set; }
    public Guid? DeviceId { get; set; }
    public string Status { get; set; }
    public string? Summary { get; set; }
    public string? CorrelationId { get; set; }
    public DateTimeOffset OccurredAtUtc { get; set; }
    public string? DetailsJson { get; set; }
}