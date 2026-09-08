using CIOT.Common.Domain;

namespace CIOT.Modules.Devices.Domain.Entities;

public sealed class FotaDownloadGrant : ICreationAuditableEntity
{
    public Guid GrantId { get; set; }
    public byte[] TokenHash { get; set; }
    public Guid UpdateSessionId { get; set; }
    public Guid AssignmentId { get; set; }
    public Guid DeviceId { get; set; }
    public Guid FirmwareArtifactId { get; set; }
    public string CertificateThumbprint { get; set; }
    public DateTimeOffset ExpiresAtUtc { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? FirstAccessedAtUtc { get; set; }
    public DateTimeOffset? LastAccessedAtUtc { get; set; }
    public DateTimeOffset? CompletedAtUtc { get; set; }

    public FotaUpdateSession? UpdateSession { get; set; }
    public DeviceAssignment? Assignment { get; set; }
    public Device Device { get; set; } = null!;
    public FirmwareArtifact FirmwareArtifact { get; set; } = null!;
}