using CIOT.Common.Domain;

namespace CIOT.Modules.Identity.Domain.Entities;

public sealed class UserAccount : BaseEntity<Guid>, ICreationAuditableEntity, IUpdateAuditableEntity, IOptimisticConcurrentEntity
{
    public string? ExternalIdentityId { get; set; }
    public string? AuthProvider { get; set; }
    public string? SapUserId { get; set; }
    public Guid? EntraTenantId { get; set; }
    public Guid? EntraObjectId { get; set; }
    public string? IdentityClaimVersion { get; set; }
    public DateTimeOffset? IdentityLastReconciledAtUtc { get; set; }

    public string Email { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public string? PhoneNumber { get; set; }

    public string UserType { get; set; } = "internal";
    public string Status { get; set; } = "pending_activation";

    public string? MainCountryCode { get; set; }

    public DateTimeOffset? LastLoginAtUtc { get; set; }
    public DateTimeOffset? ActivationEmailSentAtUtc { get; set; }
    public DateTimeOffset? ActivatedAtUtc { get; set; }
    public DateTimeOffset? ExternalIdentityBoundAtUtc { get; set; }
    public string? AttributesJson { get; set; }
    public long RowVersion { get; set; } = 1;

    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
    public DateTimeOffset? DeactivatedAtUtc { get; set; }

    public ICollection<UserRoleAssignment> RoleAssignments { get; } =
        new List<UserRoleAssignment>();
    public ICollection<UserScopeAssignment> ScopeAssignments { get; } =
        new List<UserScopeAssignment>();
}