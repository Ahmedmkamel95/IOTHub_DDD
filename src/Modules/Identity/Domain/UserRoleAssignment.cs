namespace CIOT.Modules.Identity.Domain;

public sealed class UserRoleAssignment
{
    public Guid UserId { get; set; }
    public UserAccount UserAccount { get; set; } = null!;

    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public DateTime AssignedAtUtc { get; set; } = DateTime.UtcNow;
}
