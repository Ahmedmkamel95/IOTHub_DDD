using CIOT.Common.Domain;

namespace CIOT.Modules.Identity.Domain.Entities;

public sealed class UserRoleAssignment : BaseEntity<Guid>, IEffectiveDatingEntity, ICreationAuditableEntity
{
    public Guid UserId { get; set; }
    public UserAccount User { get; set; } = null!;

    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public DateTimeOffset ValidFromUtc { get; set; }
    public DateTimeOffset? ValidToUtc { get; set; }

    public Guid? AssignedByUserId { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
}