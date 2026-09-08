using CIOT.Common.Domain;

namespace CIOT.Modules.Devices.Domain.Entities;

public sealed class Device : BaseEntity<Guid>, ICreationAuditableEntity, IUpdateAuditableEntity, IOptimisticConcurrentEntity
{
    public Guid? ManufacturerProvisioningDeviceId { get; set; }

    public string IotHubDeviceId { get; set; } = null!;

    public string? DeviceSerialNumber { get; set; }
    public string? Imei { get; set; }
    public string? Imsi { get; set; }
    public string? MacAddress { get; set; }

    public Guid? DeviceModelId { get; set; }
    public string? CountryCode { get; set; }

    public string LifecycleStatus { get; set; } = default!;
    public string? FirmwareVersion { get; set; }

    public string? SimCardNumber { get; set; }
    public string? DeactivationReason { get; set; }
    public string? InventoryModel { get; set; }
    public string? InventoryManufacturer { get; set; }
    public string? Notes { get; set; }

    public DateTimeOffset? FirstSeenAtUtc { get; set; }
    public DateTimeOffset? LastSeenAtUtc { get; set; }
    public DateTimeOffset? DeactivatedAtUtc { get; set; }

    public string? MetadataJson { get; set; }
    public long RowVersion { get; set; } = 1;

    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }

    public DeviceFirmwareState? DeviceFirmwareState { get; set; }
    public DeviceLifecycleState? DeviceLifecycleState { get; set; }

    public ICollection<DeviceAssignment> Assignments { get; } = new List<DeviceAssignment>();
}