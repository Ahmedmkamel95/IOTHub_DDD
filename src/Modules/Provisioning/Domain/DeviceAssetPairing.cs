using CIOT.Common.Domain;

namespace CIOT.Modules.Provisioning.Domain;

public sealed class DeviceAssetPairing : EntityBase
{
    public Guid DeviceId { get; set; }
    public Guid AssetId { get; set; }
    public DateTime PairedAtUtc { get; set; } = DateTime.UtcNow;
    public string PairingStatus { get; set; } = "Active";
}
