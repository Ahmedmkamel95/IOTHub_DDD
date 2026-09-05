using CIOT.Common.Domain;

namespace CIOT.Modules.Identity.Domain;

public sealed class UserAccount : AggregateRoot
{
    public string? ExternalIdentityId { get; set; } // Entra ID "oid" or "sub"
    public string? AuthProvider { get; set; } // "EntraID", "EntraExternalId", etc.
    public string? SapUserId { get; set; }

    public string Email { get; set; } = null!;
    public string? DisplayName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }

    public string UserType { get; set; } = "Internal"; // "Internal" or "External"
    public string Status { get; set; } = "Active"; // "Active", "Invited", "Suspended", "Deactivated"
    public string? MainCountryCode { get; set; }

    public DateTime? LastLoginAtUtc { get; set; }
    public DateTime? ActivatedAtUtc { get; set; }
    public DateTime? ExternalIdentityBoundAtUtc { get; set; }
    public DateTime? DeactivatedAtUtc { get; set; }

    public ICollection<UserRoleAssignment> RoleAssignments { get; } = new List<UserRoleAssignment>();
    public ICollection<UserScopeAssignment> ScopeAssignments { get; } = new List<UserScopeAssignment>();

    public void RecordLogin()
    {
        LastLoginAtUtc = DateTime.UtcNow;
    }

    public void BindExternalIdentity(string externalId, string provider)
    {
        ExternalIdentityId = externalId;
        AuthProvider = provider;
        ExternalIdentityBoundAtUtc = DateTime.UtcNow;
        if (Status == "Invited")
        {
            Status = "Active";
            ActivatedAtUtc = DateTime.UtcNow;
        }
    }
}

