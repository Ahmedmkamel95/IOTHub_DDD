using CIOT.Common.Domain;

namespace CIOT.Modules.CustomerOutlet.Domain.Entities;

public sealed class CustomerCluster : BaseEntity<Guid>, ICreationAuditableEntity, IUpdateAuditableEntity, IOptimisticConcurrentEntity
{
    public string? ClusterCode { get; set; }
    public string? CountryCode { get; set; }
    public string ClusterName { get; set; } = null!;
    public string? Description { get; set; }
    public string ClusterType { get; set; } = null!;
    public bool IsSystem { get; set; }
    public bool IsActive { get; set; } = true;
    public Guid? OwnerUserId { get; set; }
    public string Status { get; set; } = null!;
    public long RowVersion { get; set; } = 1;
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
}