using CIOT.Common.Domain;

namespace CIOT.Modules.CustomerOutlet.Domain.Entities;

public sealed class CustomerClusterMember : IEffectiveDatingEntity, ICreationAuditableEntity
{
    public Guid CustomerClusterId { get; set; }
    public Guid CustomerId { get; set; }
    public DateTimeOffset ValidFromUtc { get; set; }
    public DateTimeOffset? ValidToUtc { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
}