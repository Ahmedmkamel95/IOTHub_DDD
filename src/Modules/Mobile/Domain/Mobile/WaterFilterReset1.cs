using CIOT.Common.Domain;

namespace CIOT.Modules.Mobile.Domain.Entities;

public sealed class WaterFilterReset1 : BaseEntity<Guid>, ICreationAuditableEntity
{
    public Guid AssetId { get; set; }
    public Guid? PreviousAssetWaterFilterId { get; set; }
    public Guid AssetWaterFilterId { get; set; }
    public Guid WaterFilterModelId { get; set; }
    public DateTimeOffset ResetAtUtc { get; set; }
    public Guid ResetByUserId { get; set; }
    public string SourceApp { get; set; }
    public string? AppVersion { get; set; }
    public string? EvidenceJson { get; set; }
    public string? Notes { get; set; }
    public string? ClientActionId { get; set; }
    public string CorrelationId { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
}