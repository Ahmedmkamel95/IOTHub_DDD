using CIOT.Common.Domain;

namespace CIOT.Modules.Identity.Domain.Entities;

public sealed class ApiClientPermission : ICreationAuditableEntity
{
    public Guid ApiClientId { get; set; }
    public string PermissionCode { get; set; } = null!;
    public DateTimeOffset CreatedAtUtc { get; set; }
}