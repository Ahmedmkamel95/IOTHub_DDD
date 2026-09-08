using CIOT.Common.Domain;

namespace CIOT.Modules.Identity.Domain.Entities;

public sealed class RolePermission : ICreationAuditableEntity
{
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public string PermissionCode { get; set; } = null!;
    public Permission Permission { get; set; } = null!;

    public DateTimeOffset CreatedAtUtc { get; set; }
    public Guid? GrantedByUserId { get; set; }
}