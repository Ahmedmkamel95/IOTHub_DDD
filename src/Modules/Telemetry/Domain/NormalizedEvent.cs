using CIOT.Common.Domain;

namespace CIOT.Modules.Telemetry.Domain;

public sealed class NormalizedEvent : AggregateRoot
{
    public Guid? DeviceId { get; set; }
    public Guid? AssetId { get; set; }
    public DateTime EventOccurredAtUtc { get; set; }
    public string EventType { get; set; } = default!; // "Quality", "Vend", "Error", "Alarm"
    public string Severity { get; set; } = "Info"; // "Info", "Warning", "Critical"
    public string? Description { get; set; }
    public string? PayloadJson { get; set; }
}

