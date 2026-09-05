using CIOT.Common.Domain;

namespace CIOT.Modules.Identity.Domain;

public sealed class Permission : AggregateRoot
{
    public string Code { get; set; } = null!; // e.g. "devices:read", "telemetry:ingest"
    public string Name { get; set; } = null!;
    public string Category { get; set; } = null!; // e.g. "Devices", "Telemetry", "Admin"
    public string? Description { get; set; }

    public ICollection<RolePermission> RolePermissions { get; } = new List<RolePermission>();
}
