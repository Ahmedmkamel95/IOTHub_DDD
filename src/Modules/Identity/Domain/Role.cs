using CIOT.Common.Domain;

namespace CIOT.Modules.Identity.Domain;

public sealed class Role : AggregateRoot
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsSystemRole { get; set; }

    public ICollection<RolePermission> RolePermissions { get; } = new List<RolePermission>();
    public ICollection<UserRoleAssignment> UserAssignments { get; } = new List<UserRoleAssignment>();
}
