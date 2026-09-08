using CIOT.Common.Domain;

namespace CIOT.Modules.Devices.Domain.Entities;

public sealed class DeviceIdentifier : BaseEntity<Guid>, ICreationAuditableEntity
{
    public Guid DeviceId { get; set; }
    public string IdentifierType { get; set; }
    public string IdentifierValue { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
}