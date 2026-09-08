using CIOT.Common.Domain;

namespace CIOT.Modules.Identity.Domain.Entities;

public sealed class RolePermissionBaseline : ICreationAuditableEntity
{
    public string RoleCode { get; set; } = null!;
    public string PermissionCode { get; set; } = null!;
    public string BaselineVersion { get; set; } = null!;
    public string Rationale { get; set; } = null!;
    public DateTimeOffset CreatedAtUtc { get; set; }
}