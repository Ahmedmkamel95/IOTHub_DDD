using CIOT.Common.Domain;

namespace CIOT.Modules.Provisioning.Domain.Entities;

public sealed class DeviceModel : BaseEntity<Guid>, ICreationAuditableEntity, IUpdateAuditableEntity, IOptimisticConcurrentEntity
{
    public Guid DeviceManufacturerId { get; set; }
    public DeviceManufacturer DeviceManufacturer { get; set; } = null!;
    public string ModelCode { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string? HardwareRevision { get; set; }
    public string Status { get; set; } = default!;
    public long RowVersion { get; set; } = 1;
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
}