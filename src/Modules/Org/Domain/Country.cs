using CIOT.Common.Domain;

namespace CIOT.Modules.Org.Domain;

public sealed class Country : AggregateRoot
{
    public string CountryCode { get; set; } = null!; // ISO 2-letter
    public string CountryName { get; set; } = null!;
    public string? DefaultTimezone { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<BusinessUnit> BusinessUnits { get; } = new List<BusinessUnit>();
    public ICollection<SalesTerritory> SalesTerritories { get; } = new List<SalesTerritory>();
}

