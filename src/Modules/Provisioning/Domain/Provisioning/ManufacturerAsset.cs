using CIOT.Common.Domain;

namespace CIOT.Modules.Provisioning.Domain.Entities;

public sealed class ManufacturerAsset : BaseEntity<Guid>, ICreationAuditableEntity, IUpdateAuditableEntity, IOptimisticConcurrentEntity
{
    public Guid AssetManufacturerId { get; set; }
    public Guid EquipmentModelId { get; set; }
    public string? ManufacturerAssetReference { get; set; }
    public string ManufacturerAssetSerialNumber { get; set; }
    public string? TechnicalId { get; set; }
    public string ProvisioningStatus { get; set; }
    public Guid CreatedByUserId { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
    public long RowVersion { get; set; }
}