using CIOT.Common.Domain;

namespace CIOT.Modules.Identity.Domain.Entities;

public sealed class ActivationDeliveryOutbox : BaseEntity<Guid>, ICreationAuditableEntity, IUpdateAuditableEntity
{
    public Guid ActivationId { get; set; }
    public string RecipientEmail { get; set; } = null!;
    public string DeliveryKind { get; set; } = null!;
    public string Status { get; set; } = null!;
    public int AttemptCount { get; set; }
    public DateTimeOffset AvailableAtUtc { get; set; }
    public DateTimeOffset? SentAtUtc { get; set; }
    public string? LastErrorCode { get; set; }
    public string CorrelationId { get; set; } = null!;
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
    public string ProviderCode { get; set; } = null!;
    public string TemplateCode { get; set; } = null!;
    public DateTimeOffset? NextAttemptAtUtc { get; set; }
    public string? LeaseOwner { get; set; }
    public DateTimeOffset? LeaseUntilUtc { get; set; }
    public string? ProviderMessageId { get; set; }
    public string? PayloadSha256 { get; set; }
}