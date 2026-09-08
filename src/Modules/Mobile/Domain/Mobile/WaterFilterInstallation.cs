using CIOT.Common.Domain;

namespace CIOT.Modules.Mobile.Domain.Entities;

public sealed class WaterFilterInstallation : BaseEntity<Guid>, ICreationAuditableEntity
{
    public Guid AssetId { get; set; }
    public Guid? PreviousAssetWaterFilterId { get; set; }
    public Guid AssetWaterFilterId { get; set; }
    public Guid WaterFilterModelId { get; set; }
    public DateTimeOffset InstalledAtUtc { get; set; }
    public Guid InstalledByUserId { get; set; }
    public string? AppVersion { get; set; }
    public string? EvidenceJson { get; set; }
    public string? Notes { get; set; }
    public string? ClientActionId { get; set; }
    public string CorrelationId { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
}