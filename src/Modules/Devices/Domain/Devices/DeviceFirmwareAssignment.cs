using CIOT.Common.Domain;

namespace CIOT.Modules.Devices.Domain.Entities;

public sealed class DeviceFirmwareAssignment : BaseEntity<Guid>, ICreationAuditableEntity, IUpdateAuditableEntity
{
    public Guid DeviceId { get; set; }
    public Guid? FirmwareArtifactId { get; set; }
    public string ArtifactVersion { get; set; } = null!;
    public Guid UpdateSessionId { get; set; }
    public Guid? RequestedByUserId { get; set; }
    public DateTimeOffset RequestedAtUtc { get; set; }
    public DateTimeOffset? ScheduleAtUtc { get; set; }
    public DateTimeOffset? RolloutWindowFromUtc { get; set; }
    public DateTimeOffset? RolloutWindowToUtc { get; set; }
    public string Status { get; set; } = default!;
    public int? ProgressPercent { get; set; }
    public DateTimeOffset? LastStatusAtUtc { get; set; }
    public string? LastErrorCode { get; set; }
    public DateTimeOffset? CancelledAtUtc { get; set; }
    public string? CancellationReason { get; set; }
    public string? Notes { get; set; }
    public string? CurrentVersionAtAssignment { get; set; }
    public string? EligibilityResultJson { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
}