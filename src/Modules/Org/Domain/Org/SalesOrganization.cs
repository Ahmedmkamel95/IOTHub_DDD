using CIOT.Common.Domain;

namespace CIOT.Modules.Org.Domain.Entities;

public sealed class SalesOrganization : BaseEntity<Guid>, ICreationAuditableEntity, IUpdateAuditableEntity
{
    public string SalesOrganizationCode { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string? CountryCode { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
}