using CIOT.Common.Domain;

namespace CIOT.Modules.Identity.Domain.Entities;

public sealed class UserDeviceManufacturerAccess : IEffectiveDatingEntity, ICreationAuditableEntity
{
    public Guid UserId { get; set; }
    public Guid DeviceManufacturerId { get; set; }
    public DateTimeOffset ValidFromUtc { get; set; }
    public DateTimeOffset? ValidToUtc { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
}