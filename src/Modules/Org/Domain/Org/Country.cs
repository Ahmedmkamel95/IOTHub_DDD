using CIOT.Common.Domain;

namespace CIOT.Modules.Org.Domain.Entities;

public sealed class Country : ICreationAuditableEntity, IUpdateAuditableEntity
{
    public string CountryCode { get; set; } = null!;
    public string CountryName { get; set; } = null!;
    public string? Iso3Code { get; set; }
    public string? CurrencyCode { get; set; }
    public string? DefaultTimezone { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }

    public ICollection<BusinessUnitCountry> BusinessUnitCountries { get; } = new List<BusinessUnitCountry>();
    public ICollection<SalesTerritory> SalesTerritories { get; } = new List<SalesTerritory>();
}