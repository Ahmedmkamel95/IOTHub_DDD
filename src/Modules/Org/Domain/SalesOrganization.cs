using CIOT.Common.Domain;

namespace CIOT.Modules.Org.Domain;

public sealed class SalesOrganization : AggregateRoot
{
    public string SalesOrganizationCode { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string? CountryCode { get; set; }
    public bool IsActive { get; set; } = true;
}

