using CIOT.Common.Domain;

namespace CIOT.Modules.CustomerOutlet.Domain;

public sealed class Outlet : AggregateRoot
{
    public string OutletCode { get; set; } = null!;

    public Guid? CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public string? OutletType { get; set; }
    public string? AddressLine { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public string CountryCode { get; set; } = null!;

    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }

    public Guid? SalesTerritoryId { get; set; } // Reference to Org.SalesTerritory

    public string? SubTradeChannel { get; set; }
    public string? Segmentation { get; set; }
    public string? Seasonality { get; set; }
    public string? PayerCode { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<OutletNote> Notes { get; } = new List<OutletNote>();
}

