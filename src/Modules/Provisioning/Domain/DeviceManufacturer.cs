using CIOT.Common.Domain;

namespace CIOT.Modules.Provisioning.Domain;

public sealed class DeviceManufacturer : AggregateRoot
{
    public string ManufacturerCode { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string Status { get; set; } = "Active";

    public ICollection<DeviceModel> DeviceModels { get; } = new List<DeviceModel>();
}
