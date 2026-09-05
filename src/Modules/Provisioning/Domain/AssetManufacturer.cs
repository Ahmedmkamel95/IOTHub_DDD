using CIOT.Common.Domain;

namespace CIOT.Modules.Provisioning.Domain;

public sealed class AssetManufacturer : AggregateRoot
{
    public string ManufacturerCode { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string Status { get; set; } = "Active";
}
