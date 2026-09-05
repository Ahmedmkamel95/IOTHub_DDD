namespace CIOT.Modules.Identity.Domain;

public sealed class AuthProviderConfig
{
    public string ProviderKey { get; set; } = null!; // e.g. "EntraID-Internal", "EntraExternalId"
    public string DisplayName { get; set; } = null!;
    public string AudienceType { get; set; } = "Api";
    public string Protocol { get; set; } = "OpenIDConnect";
    public string IssuerUrl { get; set; } = null!;
    public string? ClientId { get; set; }
    public string? TenantId { get; set; }
    public string SubjectClaim { get; set; } = "oid";
    public bool IsEnabled { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}

