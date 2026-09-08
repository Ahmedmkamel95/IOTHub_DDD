using CIOT.Common.Domain;

namespace CIOT.Modules.Identity.Domain.Entities;

public sealed class ApiClient : BaseEntity<Guid>, ICreationAuditableEntity, IUpdateAuditableEntity
{
    public string EntraClientId { get; set; } = null!;
    public string ClientName { get; set; } = null!;
    public string ClientType { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
}