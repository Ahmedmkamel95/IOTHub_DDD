using CIOT.Common.Domain;

namespace CIOT.Modules.Asset.Domain.Entities;

public sealed class Asset : BaseEntity<Guid>, ICreationAuditableEntity, IUpdateAuditableEntity, IOptimisticConcurrentEntity
{
    public Guid? ManufacturerProvisioningAssetId { get; set; }

    public string CountryCode { get; set; } = null!;

    public Guid? CustomerId { get; set; }

    public Guid? CurrentOutletId { get; set; }

    public Guid? EquipmentModelId { get; set; }
    public string? AssetTypeCode { get; set; }

    public string SapEquipmentNumber { get; set; } = null!;
    public string? OemSerialNumber { get; set; }
    public string? TechnicalId { get; set; }
    public string? Barcode { get; set; }

    public string Status { get; set; } = "active";

    public DateTimeOffset? LastConnectionAtUtc { get; set; }

    public string? SourcePayloadJson { get; set; }

    public string SourceSystem { get; set; } = default!;
    public string? MachineType { get; set; }
    public string? Manufacturer { get; set; }
    public string? Model { get; set; }
    public bool SupportsDevicelessTelemetry { get; set; }
    public string TelemetrySourceMode { get; set; } = default!;
    public long RowVersion { get; set; } = 1;
    public string? MachineGroup { get; set; }
    public string? CoffeeBrandCode { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
}