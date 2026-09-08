using CIOT.Common.Domain;

namespace CIOT.Modules.Mobile.Domain.Entities;

public sealed class DosageChange : BaseEntity<Guid>, ICreationAuditableEntity
{
    public Guid AssetId { get; set; }
    public Guid ChangedByUserId { get; set; }
    public DateTimeOffset ChangedAtUtc { get; set; }
    public string DosagesJson { get; set; }
    public string? BeforeDosagesJson { get; set; }
    public string? Reason { get; set; }
    public string? AppVersion { get; set; }
    public string? ClientActionId { get; set; }
    public string CorrelationId { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
}