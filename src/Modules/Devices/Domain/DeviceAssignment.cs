using CIOT.Common.Domain;

namespace CIOT.Modules.Devices.Domain;

public sealed class DeviceAssignment : EntityBase
{
    public Guid DeviceId { get; set; }
    public Device Device { get; set; } = null!;

    public Guid AssetId { get; set; } // References Asset.Asset
    public string AssignmentType { get; set; } = "Production";

    public DateTime PairedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UnpairedAtUtc { get; set; }
    public bool IsActive => UnpairedAtUtc == null;

    public void Unpair()
    {
        UnpairedAtUtc = DateTime.UtcNow;
    }
}
