using CIOT.Common.Domain;

namespace CIOT.Modules.Devices.Domain;

public sealed class Device : AggregateRoot
{
    public string IotHubDeviceId { get; set; } = null!;
    public string? DeviceSerialNumber { get; set; }
    public string? Imei { get; set; }
    public string? Imsi { get; set; }
    public string? MacAddress { get; set; }

    public Guid? DeviceModelId { get; set; }
    public string? CountryCode { get; set; }

    public string LifecycleStatus { get; set; } = "Registered"; // "Registered", "Active", "Suspended", "Decommissioned"
    public string? FirmwareVersion { get; set; }

    public DateTime? FirstSeenAtUtc { get; set; }
    public DateTime? LastSeenAtUtc { get; set; }
    public string? MetadataJson { get; set; }

    public ICollection<DeviceAssignment> Assignments { get; } = new List<DeviceAssignment>();
    public ICollection<DeviceCommand> Commands { get; } = new List<DeviceCommand>();

    public void RecordHeartbeat(string? firmwareVersion = null)
    {
        LastSeenAtUtc = DateTime.UtcNow;
        FirstSeenAtUtc ??= DateTime.UtcNow;
        if (!string.IsNullOrEmpty(firmwareVersion))
        {
            FirmwareVersion = firmwareVersion;
        }
    }

    public void Activate() => LifecycleStatus = "Active";
    public void Suspend() => LifecycleStatus = "Suspended";
    public void Decommission() => LifecycleStatus = "Decommissioned";
}

