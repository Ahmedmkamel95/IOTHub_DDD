using CIOT.Common.Domain;
using System.Drawing;


namespace CIOT.Modules.CustomerOutlet.Domain.Entities;

public sealed class Outlet : BaseEntity<Guid>, ICreationAuditableEntity, IUpdateAuditableEntity, IOptimisticConcurrentEntity
{
    public string CountryCode { get; set; } = null!;
    public string OutletCode { get; set; } = null!;

    public Guid? CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public string OutletName { get; set; } = null!;

    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? AddressLine { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public bool IsActive { get; set; } = true;

    public Point? Location { get; set; }

    public Guid? SalesTerritoryId { get; set; }
    public Guid? WholesalerCustomerId { get; set; }

    public string Status { get; set; } = default!;

    public string SourceSystem { get; set; } = default!;
    public string? SourcePayloadJson { get; set; }
    public string? OutletType { get; set; }
    public string? SubTradeChannel { get; set; }
    public string? CoffeeSegmentCode { get; set; }
    public string? BrandCode { get; set; }
    public long RowVersion { get; set; } = 1;

    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
}