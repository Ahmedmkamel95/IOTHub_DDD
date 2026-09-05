using CIOT.Common.Domain;

namespace CIOT.Modules.Integration.Domain;

public sealed class PartnerSource : AggregateRoot
{
    public string SourceCode { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string IntegrationType { get; set; } = "REST"; // "REST", "SFTP", "Webhook"
    public string? EndpointUrl { get; set; }
    public bool IsActive { get; set; } = true;
}
