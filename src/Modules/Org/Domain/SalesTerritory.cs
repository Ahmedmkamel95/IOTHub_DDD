using CIOT.Common.Domain;

namespace CIOT.Modules.Org.Domain;

public sealed class SalesTerritory : AggregateRoot
{
    public string TerritoryCode { get; set; } = null!;
    public string? TerritoryName { get; set; }

    public string CountryCode { get; set; } = null!;
    public Country Country { get; set; } = null!;

    public bool IsActive { get; set; } = true;
}

