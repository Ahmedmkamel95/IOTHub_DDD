using CIOT.Common.Domain;

namespace CIOT.Modules.LocalAdapter.Domain;

public sealed class DeviceProjectionEffect : AggregateRoot
{
    public Guid DeviceId { get; set; }
    public string EffectType { get; set; } = "ConfigUpdate"; // "ConfigUpdate", "FirmwareSync", "CommandRelay"
    public string EffectPayloadJson { get; set; } = "{}";
    public DateTime AppliedAtUtc { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Applied";
}

