using CIOT.Common.Domain;

namespace CIOT.Modules.Integration.Domain;

public sealed class PartnerRawOutbox : AggregateRoot
{
    public string DestinationPartnerCode { get; set; } = null!;
    public string EventType { get; set; } = null!;
    public string PayloadJson { get; set; } = null!;
    public string Status { get; set; } = "Pending"; // "Pending", "Dispatched", "Failed"
    public int RetryCount { get; set; }
}
