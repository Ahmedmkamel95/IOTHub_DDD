using CIOT.Common.Domain;

namespace CIOT.Modules.Asset.Domain;

public sealed class AssetWaterFilter : EntityBase
{
    public Guid AssetId { get; set; }
    public Asset Asset { get; set; } = null!;

    public Guid? WaterFilterModelId { get; set; }
    public DateTime? InstalledAtUtc { get; set; }
    public DateTime? LastResetAtUtc { get; set; }
    public Guid? LastResetByUserId { get; set; }
    public decimal? CapacityUsedLiters { get; set; }
    public DateTime? ExpiresAtUtc { get; set; }
    public string Status { get; set; } = "Active";
    public bool IsCurrent { get; set; } = true;
}
