using CIOT.Common.Domain;

namespace CIOT.Modules.CustomerOutlet.Domain.Entities;

public sealed class CustomerRelationship : BaseEntity<Guid>, IEffectiveDatingEntity, ICreationAuditableEntity
{
    public Guid ParentCustomerId { get; set; }
    public Guid ChildCustomerId { get; set; }
    public string RelationshipType { get; set; } = null!;
    public DateTimeOffset ValidFromUtc { get; set; }
    public DateTimeOffset? ValidToUtc { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
}