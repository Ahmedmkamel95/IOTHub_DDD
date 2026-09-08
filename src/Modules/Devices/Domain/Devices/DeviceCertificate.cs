using CIOT.Common.Domain;

namespace CIOT.Modules.Devices.Domain.Entities;

public sealed class DeviceCertificate : BaseEntity<Guid>, ICreationAuditableEntity
{
    public Guid DeviceId { get; set; }
    public string Thumbprint { get; set; } = null!;
    public string? Subject { get; set; }
    public string? Issuer { get; set; }
    public string? SerialNumber { get; set; }
    public DateTimeOffset? IssuedAtUtc { get; set; }
    public DateTimeOffset? ExpiresAtUtc { get; set; }
    public string Status { get; set; } = default!;
    public DateTimeOffset? RevokedAtUtc { get; set; }
    public string? RevocationReasonCode { get; set; }
    public string? RevocationReason { get; set; }
    public bool? RecoveryAllowed { get; set; }
    public Guid? SupersededByCertificateId { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
}