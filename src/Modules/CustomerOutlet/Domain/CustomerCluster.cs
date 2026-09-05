using CIOT.Common.Domain;

namespace CIOT.Modules.CustomerOutlet.Domain;

public sealed class CustomerCluster : AggregateRoot
{
    public string ClusterCode { get; set; } = null!;
    public string ClusterName { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}
