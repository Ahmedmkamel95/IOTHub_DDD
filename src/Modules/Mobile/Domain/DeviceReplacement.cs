using CIOT.Common.Domain;

namespace CIOT.Modules.Mobile.Domain;

public sealed class DeviceReplacement : AggregateRoot
{
    public Guid AssetId { get; set; }
    public Guid OldDeviceId { get; set; }
    public Guid NewDeviceId { get; set; }
    public Guid ReplacedByUserId { get; set; }
    public string? Reason { get; set; }
    public DateTime ReplacedAtUtc { get; set; } = DateTime.UtcNow;
}
