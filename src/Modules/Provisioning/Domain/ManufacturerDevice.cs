using CIOT.Common.Domain;

namespace CIOT.Modules.Provisioning.Domain;

public sealed class ManufacturerDevice : AggregateRoot
{
    public Guid DeviceModelId { get; set; }
    public string SerialNumber { get; set; } = null!;
    public string? MacAddress { get; set; }
    public string? Imei { get; set; }
    public string ProvisioningStatus { get; set; } = "Created"; // "Created", "Provisioned", "Paired"
}
