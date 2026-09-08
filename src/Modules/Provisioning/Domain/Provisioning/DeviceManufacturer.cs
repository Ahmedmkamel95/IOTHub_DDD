using CIOT.Common.Domain;

namespace CIOT.Modules.Provisioning.Domain.Entities;

public sealed class DeviceManufacturer : BaseEntity<Guid>, ICreationAuditableEntity, IUpdateAuditableEntity
{
    public string ManufacturerCode { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
}