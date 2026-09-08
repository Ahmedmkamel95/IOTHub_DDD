using CIOT.Common.Domain;

namespace CIOT.Modules.Devices.Domain.Entities;

public sealed class DeviceFirmwareStatusEvent : BaseEntity<Guid>, ICreationAuditableEntity
{
    public Guid? FirmwareAssignmentId { get; set; }
    public Guid DeviceId { get; set; }
    public DateTimeOffset EventAtUtc { get; set; }
    public string Status { get; set; }
    public int? ProgressPercent { get; set; }
    public string? Message { get; set; }
    public string? PayloadJson { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
}