using CIOT.Common.Domain;

namespace CIOT.Modules.Provisioning.Domain;

public sealed class DeviceModel : EntityBase
{
    public Guid DeviceManufacturerId { get; set; }
    public DeviceManufacturer DeviceManufacturer { get; set; } = null!;

    public string ModelCode { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string? HardwareRevision { get; set; }
    public string Status { get; set; } = "Active";
}
