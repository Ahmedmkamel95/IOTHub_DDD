using CIOT.Common.Domain;

namespace CIOT.Modules.Asset.Domain.Entities;

public sealed class WaterFilterReset : BaseEntity<Guid>, ICreationAuditableEntity
{
    public Guid AssetId { get; set; }

    public Guid? AssetWaterFilterId { get; set; }

    public DateTimeOffset ResetAtUtc { get; set; }

    public Guid? ResetByUserId { get; set; }

    public string? SourceApp { get; set; }

    public string? EvidenceJson { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }
}