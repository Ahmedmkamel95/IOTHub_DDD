using CIOT.Common.Domain;

namespace CIOT.Modules.Admin.Domain;

public sealed class OperationalStatusPolicy : AggregateRoot
{
    public string PolicyName { get; set; } = null!;
    public string EquipmentModelCode { get; set; } = null!;
    public int HeartbeatTimeoutMinutes { get; set; } = 30;
    public string StatusWhenOffline { get; set; } = "Offline";
}
