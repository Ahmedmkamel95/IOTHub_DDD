using CIOT.Common.Domain;

namespace CIOT.Modules.CustomerOutlet.Domain;

public sealed class CustomerRelationship : EntityBase
{
    public Guid PrimaryCustomerId { get; set; }
    public Guid RelatedCustomerId { get; set; }
    public string RelationshipType { get; set; } = null!; // e.g. "ParentChild", "Affiliate"
}
