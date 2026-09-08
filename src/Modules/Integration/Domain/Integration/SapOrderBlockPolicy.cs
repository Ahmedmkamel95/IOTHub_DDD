using CIOT.Common.Domain;

namespace CIOT.Modules.Integration.Domain.Entities;

public sealed class SapOrderBlockPolicy : BaseEntity<Guid>, IOptimisticConcurrentEntity, IEffectiveDatingEntity, ICreationAuditableEntity, IUpdateAuditableEntity
{
    public string SourceSystem { get; set; } = null!;
    public string OrderBlockCode { get; set; } = null!;
    public bool IsBlocked { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; } = null!;
    public DateTimeOffset ValidFromUtc { get; set; }
    public DateTimeOffset? ValidToUtc { get; set; }
    public long RowVersion { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
}