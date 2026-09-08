using CIOT.Common.Domain;

namespace CIOT.Modules.CustomerOutlet.Domain.Entities;

public sealed class Customer : BaseEntity<Guid>, ICreationAuditableEntity, IUpdateAuditableEntity, IOptimisticConcurrentEntity
{
    public string CountryCode { get; set; } = null!;
    public string CustomerCode { get; set; } = null!;
    public string? CustomerName1 { get; set; }
    public string? CustomerName2 { get; set; }
    public string? VatNumber { get; set; }
    public string? CustomerType { get; set; }
    public Guid? SalesTerritoryId { get; set; }
    public string? WholesalerCode { get; set; }
    public Guid? CustomerClusterId { get; set; }
    public CustomerCluster? CustomerCluster { get; set; }
    public bool IsActive { get; set; } = true;
    public string Status { get; set; } = default!;
    public string SourceSystem { get; set; } = default!;
    public string? SourcePayloadJson { get; set; }
    public string? SapOrderBlockCode { get; set; }
    public string? CoffeeSegmentationCode { get; set; }
    public long RowVersion { get; set; } = 1;
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }

    public ICollection<Outlet> Outlets { get; } = new List<Outlet>();
}