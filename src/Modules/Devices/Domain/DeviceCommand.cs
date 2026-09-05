using CIOT.Common.Domain;

namespace CIOT.Modules.Devices.Domain;

public sealed class DeviceCommand : EntityBase
{
    public Guid DeviceId { get; set; }
    public Device Device { get; set; } = null!;

    public string CommandType { get; set; } = null!;
    public string PayloadJson { get; set; } = "{}";

    public string DeliveryPath { get; set; } = "C2D"; // "C2D", "DirectMethod"
    public string Status { get; set; } = "Enqueued"; // "Enqueued", "Sent", "Acknowledged", "Failed"

    public string? IotHubMessageId { get; set; }
    public DateTime? EnqueuedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAtUtc { get; set; }
    public string? LastError { get; set; }

    public void MarkCompleted()
    {
        Status = "Acknowledged";
        CompletedAtUtc = DateTime.UtcNow;
    }

    public void MarkFailed(string error)
    {
        Status = "Failed";
        LastError = error;
        CompletedAtUtc = DateTime.UtcNow;
    }
}
