using CIOT.Common.Domain;

namespace CIOT.Modules.Provisioning.Domain.Entities;

public sealed class ManufacturerDevice : BaseEntity<Guid>, ICreationAuditableEntity, IUpdateAuditableEntity, IOptimisticConcurrentEntity
{
    public Guid DeviceManufacturerId { get; set; }
    public Guid DeviceModelId { get; set; }
    public Guid? AssetManufacturerId { get; set; }
    public string? ManufacturerDeviceReference { get; set; }
    public string DeviceSerialNumber { get; set; }
    public string Imei { get; set; }
    public string? Iccid { get; set; }
    public string? Imsi { get; set; }
    public string? Msisdn { get; set; }
    public string? MacAddress { get; set; }
    public string? CurrentFirmwareVersion { get; set; }
    public string? BatchNumber { get; set; }
    public string? OrderNumber { get; set; }
    public string ProvisioningStatus { get; set; }
    public Guid? CreatedByUserId { get; set; }
    public Guid? CreatedByApiClientId { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
    public long RowVersion { get; set; }
}