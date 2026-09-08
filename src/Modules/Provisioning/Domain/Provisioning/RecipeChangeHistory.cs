using CIOT.Common.Domain;

namespace CIOT.Modules.Provisioning.Domain.Entities;

public sealed class RecipeChangeHistory : BaseEntity<Guid>
{
    public Guid AssetId { get; set; }
    public string RecipeCode { get; set; } = default!;
    public Guid? ChangedByUserId { get; set; }
    public string? SourceApp { get; set; }
    public string? BeforeJson { get; set; }
    public string AfterJson { get; set; } = default!;
    public DateTimeOffset ChangedAtUtc { get; set; }
    public string? Reason { get; set; }
    public string? EvidenceJson { get; set; }
    public string? AppVersion { get; set; }
    public string? ClientActionId { get; set; }
    public string? CorrelationId { get; set; }
    public string? DesiredPayloadSha256 { get; set; }
    public string DeliveryStatus { get; set; } = default!;
    public DateTimeOffset? AcknowledgedAtUtc { get; set; }
    public string? AcknowledgementCorrelationId { get; set; }
    public string? FailureCode { get; set; }
}