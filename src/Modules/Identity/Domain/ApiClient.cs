using CIOT.Common.Domain;

namespace CIOT.Modules.Identity.Domain;

public sealed class ApiClient : AggregateRoot
{
    public string ClientId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string ClientSecretHash { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public string? Description { get; set; }
    public string? AllowedIpRanges { get; set; }
}

