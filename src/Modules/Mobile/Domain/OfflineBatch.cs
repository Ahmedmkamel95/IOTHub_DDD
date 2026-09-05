using CIOT.Common.Domain;

namespace CIOT.Modules.Mobile.Domain;

public sealed class OfflineBatch : AggregateRoot
{
    public Guid TechnicianUserId { get; set; }
    public string DeviceClientSessionId { get; set; } = null!;
    public int ActionCount { get; set; }
    public string Status { get; set; } = "Completed"; // "Processing", "Completed", "PartialFailure"
    public DateTime ProcessedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<OfflineActionResult> Results { get; } = new List<OfflineActionResult>();
}
