using CIOT.Common.Domain;

namespace CIOT.Modules.Asset.Domain.Entities;

public sealed class AssetTarget : BaseEntity<Guid>, IEffectiveDatingEntity, ICreationAuditableEntity
{
    public Guid AssetId { get; set; }
    public string MetricKey { get; set; } = null!;
    public decimal TargetValue { get; set; }
    public string Unit { get; set; } = null!;
    public string? Reason { get; set; }
    public DateTimeOffset ValidFromUtc { get; set; }
    public DateTimeOffset? ValidToUtc { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
}