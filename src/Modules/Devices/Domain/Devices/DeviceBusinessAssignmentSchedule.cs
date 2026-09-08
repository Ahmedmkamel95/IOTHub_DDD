using CIOT.Common.Domain;

namespace CIOT.Modules.Devices.Domain.Entities;

public sealed class DeviceBusinessAssignmentSchedule : BaseEntity<Guid>, ICreationAuditableEntity
{
    public Guid DeviceId { get; set; }
    public Guid EquipmentId { get; set; }
    public Guid? OutletId { get; set; }
    public Guid? CustomerId { get; set; }
    public string CountryCode { get; set; } = default!;
    public string BusinessTagsJson { get; set; } = default!;
    public string? Notes { get; set; }
    public DateTimeOffset EffectiveFromUtc { get; set; }
    public string Status { get; set; }
    public Guid RequestedByUserId { get; set; }
    public string CorrelationId { get; set; }
    public short AttemptCount { get; set; }
    public DateTimeOffset NextAttemptAtUtc { get; set; }
    public string? LastErrorCode { get; set; }
    public string? LastErrorMessage { get; set; }
    public Guid? OutboxId { get; set; }
    public Guid? SupersededByScheduleId { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? ActivatedAtUtc { get; set; }
    public DateTimeOffset? CancelledAtUtc { get; set; }
    public Guid? CancelledByUserId { get; set; }
    public DateTimeOffset? SupersededAtUtc { get; set; }
}