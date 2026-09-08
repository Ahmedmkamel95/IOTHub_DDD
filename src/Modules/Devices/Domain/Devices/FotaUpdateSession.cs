using CIOT.Common.Domain;

namespace CIOT.Modules.Devices.Domain.Entities;

public sealed class FotaUpdateSession : ICreationAuditableEntity, IUpdateAuditableEntity
{
    public Guid UpdateSessionId { get; set; }
    public Guid AssignmentId { get; set; }
    public Guid DeviceId { get; set; }
    public Guid FirmwareArtifactId { get; set; }
    public string Status { get; set; }
    public short AttemptCount { get; set; }
    public short MaxAttempts { get; set; }
    public DateTimeOffset? ManifestRequestedAtUtc { get; set; }
    public DateTimeOffset? DownloadUrlExpiresAtUtc { get; set; }
    public int? EffectiveChunkSizeBytes { get; set; }
    public string? AuditReference { get; set; }
    public string? LastErrorCode { get; set; }
    public bool? BootConfirmed { get; set; }
    public string? ReportedFirmwareVersion { get; set; }
    public string? ReportedFirmwareSha256 { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
    public DateTimeOffset? PostRebootStatusAtUtc { get; set; }
    public string? PostRebootTransport { get; set; }

    public DeviceAssignment? Assignment { get; set; }
    public Device Device { get; set; } = null!;
    public FirmwareArtifact FirmwareArtifact { get; set; } = null!;
}