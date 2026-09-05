using CIOT.Common.Domain;

namespace CIOT.Modules.Telemetry.Domain;

public sealed class RawMessage : AggregateRoot
{
    public string MessageId { get; set; } = Guid.NewGuid().ToString();
    public string? DeviceIdentifier { get; set; }
    public string? AssetIdentifier { get; set; }
    public DateTime ReceivedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? SentAtUtc { get; set; }
    public string PayloadJson { get; set; } = default!;
    public string Status { get; set; } = "Received"; // "Received", "Processed", "Failed"
    public string? CorrelationId { get; set; }
    public string? PayloadHash { get; set; }
}

