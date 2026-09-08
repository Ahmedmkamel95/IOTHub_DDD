using CIOT.Common.Domain;

namespace CIOT.Modules.CustomerOutlet.Domain.Entities;

public sealed class OutletAssignmentHistory : BaseEntity<Guid>, IEffectiveDatingEntity, ICreationAuditableEntity
{
    public Guid OutletId { get; set; }
    public Guid CustomerId { get; set; }
    public DateTimeOffset ValidFromUtc { get; set; }
    public DateTimeOffset? ValidToUtc { get; set; }
    public string SourceSystem { get; set; } = null!;
    public DateTimeOffset CreatedAtUtc { get; set; }
}