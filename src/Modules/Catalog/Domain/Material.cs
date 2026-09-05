using CIOT.Common.Domain;

namespace CIOT.Modules.Catalog.Domain;

public sealed class Material : AggregateRoot
{
    public string MaterialCode { get; set; } = null!;
    public string? ProductBaseName { get; set; }
    public string? ProductName { get; set; }
    public string CountryCode { get; set; } = null!;
    public Guid? BusinessUnitId { get; set; }
    public string? CustomerVat { get; set; }
    public bool IsActive { get; set; } = true;
}
