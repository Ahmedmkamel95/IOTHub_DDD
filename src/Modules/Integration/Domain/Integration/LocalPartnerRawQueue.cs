using CIOT.Common.Domain;

namespace CIOT.Modules.Integration.Domain.Entities;

public sealed class LocalPartnerRawQueue : ICreationAuditableEntity, IUpdateAuditableEntity
{
    public Guid OutboxId { get; set; }
    public string PartnerCode { get; set; } = null!;
    public string SourceEntityKey { get; set; } = null!;
    public string PackageJson { get; set; } = null!;
    public string Status { get; set; } = null!;
    public int DeliveryCount { get; set; }
    public string? LeaseOwner { get; set; }
    public DateTimeOffset? LeaseUntilUtc { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTimeOffset? CompletedAtUtc { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
}