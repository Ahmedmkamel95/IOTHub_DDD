using CIOT.Common.Domain;

namespace CIOT.Modules.Identity.Domain;

public sealed class UserScopeAssignment : EntityBase
{
    public Guid UserId { get; set; }
    public UserAccount UserAccount { get; set; } = null!;

    public string ScopeType { get; set; } = null!; // e.g. "BusinessUnit", "Country"
    public string ScopeValue { get; set; } = null!; // e.g. "BU-EU-WEST", "DE"
}
