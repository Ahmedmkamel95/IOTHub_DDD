using CIOT.Common.Domain;

namespace CIOT.Modules.Asset.Domain.Entities;

public sealed class AssetWaterFilter : BaseEntity<Guid>, ICreationAuditableEntity
{
    public Guid AssetId { get; set; }

    public Guid WaterFilterModelId { get; set; }

    public string? FilterSerialNumber { get; set; }

    public decimal? InitialCapacityL { get; set; }

    public DateTimeOffset InstalledAtUtc { get; set; }

    public Guid? InstalledByUserId { get; set; }

    public decimal? RemainingL { get; set; }

    public string Status { get; set; } = default!;

    public DateTimeOffset? RemovedAtUtc { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }
}