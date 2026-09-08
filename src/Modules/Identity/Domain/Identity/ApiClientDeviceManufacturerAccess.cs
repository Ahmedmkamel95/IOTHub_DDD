using CIOT.Common.Domain;

namespace CIOT.Modules.Identity.Domain.Entities;

public sealed class ApiClientDeviceManufacturerAccess : ICreationAuditableEntity
{
    public Guid ApiClientId { get; set; }
    public Guid DeviceManufacturerId { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
}