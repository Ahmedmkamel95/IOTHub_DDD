using CIOT.Common.Domain;

namespace CIOT.Modules.Asset.Domain.Entities;

public sealed class AssetExternalIdentity : BaseEntity<Guid>, ICreationAuditableEntity, IUpdateAuditableEntity, IEffectiveDatingEntity, IOptimisticConcurrentEntity
{
    public string? SourceSystem { get; set; }
    public Guid AssetId { get; set; }
    public Asset Asset { get; set; } = null!;

    public string? VendorId { get; set; }

    public string? ExternalAssetId { get; set; }
    public string? ExternalDeviceId { get; set; }
    public string? ExternalSerialNumber { get; set; }

    public string IdentityType { get; set; } = null!;
    public string IdentityValue { get; set; } = null!;

    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }

    public string? MetadataJson { get; set; }
    public long RowVersion { get; set; } = 1;

    public DateTimeOffset ValidFromUtc { get; set; }
    public DateTimeOffset? ValidToUtc { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
}