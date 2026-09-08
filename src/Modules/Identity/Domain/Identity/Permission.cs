using CIOT.Common.Domain;

namespace CIOT.Modules.Identity.Domain.Entities;

public sealed class Permission : ICreationAuditableEntity
{
    public string PermissionCode { get; set; } = null!;
    public string Category { get; set; } = null!;
    public string Action { get; set; } = null!;
    public string? Description { get; set; }
    public string? AllowedScopeTypes { get; set; }
    public bool IsSystemPermission { get; set; } = true;
    public string RiskLevel { get; set; } = default!;
    public bool IsAssignable { get; set; } = true;
    public DateTimeOffset CreatedAtUtc { get; set; }
}