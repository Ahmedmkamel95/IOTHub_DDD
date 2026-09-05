using CIOT.Common.Domain;

namespace CIOT.Modules.Admin.Domain;

public sealed class EquipmentModel : AggregateRoot
{
    public string Manufacturer { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string? Submodel { get; set; }
    public string MachineType { get; set; } = null!;
    public bool SupportsPhysicalDevice { get; set; } = true;
    public bool RecipeSupported { get; set; } = true;
    public bool IsActive { get; set; } = true;
}
