using CIOT.Common.Domain;

namespace CIOT.Modules.Catalog.Domain;

public sealed class SkuMapping : EntityBase
{
    public string SkuCode { get; set; } = null!;
    public Guid MaterialId { get; set; }
    public string MappingType { get; set; } = "Default";
    public bool IsActive { get; set; } = true;
}
