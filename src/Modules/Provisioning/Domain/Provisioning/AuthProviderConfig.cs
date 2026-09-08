using CIOT.Common.Domain;

namespace CIOT.Modules.Provisioning.Domain.Entities;

public sealed class AuthProviderConfig : ICreationAuditableEntity, IUpdateAuditableEntity
{
    public string ProviderKey { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string AudienceType { get; set; } = null!;
    public string Protocol { get; set; } = null!;
    public string IssuerUrl { get; set; } = null!;
    public string? ClientId { get; set; }
    public string? TenantId { get; set; }
    public string SubjectClaim { get; set; } = null!;
    public string? MetadataJson { get; set; }
    public bool IsEnabled { get; set; }
    public int SortOrder { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
}