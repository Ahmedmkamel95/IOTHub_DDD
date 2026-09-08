using CIOT.Common.Domain;

namespace CIOT.Modules.Provisioning.Domain.Entities;

public sealed class DeviceAssetPairing : BaseEntity<Guid>, ICreationAuditableEntity
{
    public Guid ManufacturerDeviceId { get; set; }
    public Guid ManufacturerAssetId { get; set; }
    public string PairingStatus { get; set; }
    public Guid PairedByUserId { get; set; }
    public DateTimeOffset PairedAtUtc { get; set; }
    public Guid? CancelledByUserId { get; set; }
    public DateTimeOffset? CancelledAtUtc { get; set; }
    public DateTimeOffset? SapConfirmedAtUtc { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
}