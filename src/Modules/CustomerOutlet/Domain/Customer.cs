using CIOT.Common.Domain;

namespace CIOT.Modules.CustomerOutlet.Domain;

public sealed class Customer : AggregateRoot
{
    public string CustomerCode { get; set; } = null!;
    public string? CustomerName1 { get; set; }
    public string? CustomerName2 { get; set; }
    public string CountryCode { get; set; } = null!;
    public string? VatNumber { get; set; }
    public string? WholesalerCode { get; set; }
    public bool IsActive { get; set; } = true;

    public Guid? CustomerClusterId { get; set; }
    public CustomerCluster? CustomerCluster { get; set; }

    public ICollection<Outlet> Outlets { get; } = new List<Outlet>();
}

