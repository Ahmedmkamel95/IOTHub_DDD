using CIOT.Common.Domain;

namespace CIOT.Modules.Devices.Domain.Entities;

public sealed class DeviceBulkActionItem : BaseEntity<Guid>, ICreationAuditableEntity
{
    public Guid BulkActionId { get; set; }
    public Guid DeviceId { get; set; }
    public string Status { get; set; }
    public string? ResultJson { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? CompletedAtUtc { get; set; }
}