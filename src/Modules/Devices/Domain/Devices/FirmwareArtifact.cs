using CIOT.Common.Domain;

namespace CIOT.Modules.Devices.Domain.Entities;

public sealed class FirmwareArtifact : BaseEntity<Guid>, ICreationAuditableEntity, IUpdateAuditableEntity, IOptimisticConcurrentEntity
{
    public string FirmwareFamilyCode { get; set; }
    public string FirmwareVersion { get; set; }
    public string? HardwareRevision { get; set; }
    public string ArtifactUri { get; set; }
    public string StorageObjectName { get; set; }
    public string? FileName { get; set; }
    public string Sha256 { get; set; }
    public string? Signature { get; set; }
    public string? SignatureAlgorithm { get; set; }
    public long SizeBytes { get; set; }
    public string? ReleaseNotes { get; set; }
    public bool RequiredUpdate { get; set; }
    public string Status { get; set; }
    public Guid? CreatedByUserId { get; set; }
    public Guid? PublishedByUserId { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? PublishedAtUtc { get; set; }
    public DateTimeOffset? RetiredAtUtc { get; set; }
    public string? MetadataJson { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
    public long RowVersion { get; set; }
    public string StorageContainer { get; set; }
    public string ContentType { get; set; }
    public string MalwareScanStatus { get; set; }
    public string? MalwareScanProvider { get; set; }
    public string? IdempotencyKey { get; set; }
    public string? MalwareScanReference { get; set; }
    public DateTimeOffset? MalwareScanCompletedAtUtc { get; set; }
    public DateTimeOffset? SignatureVerifiedAtUtc { get; set; }
    public DateTimeOffset? UploadCompletedAtUtc { get; set; }
    public DateTimeOffset? QuarantineExpiresAtUtc { get; set; }
    public string? RejectionReasonCode { get; set; }
    public string? RejectionReason { get; set; }

    public ICollection<FirmwareArtifactDeviceModel> FirmwareArtifactDeviceModels { get; } =
        new List<FirmwareArtifactDeviceModel>();
    public ICollection<FirmwareArtifactEquipmentModel> FirmwareArtifactEquipmentModels { get; } =
        new List<FirmwareArtifactEquipmentModel>();
    public ICollection<FotaDownloadGrant> FotaDownloadGrants { get; } =
        new List<FotaDownloadGrant>();
    public ICollection<FotaUpdateSession> FotaUpdateSessions { get; } =
        new List<FotaUpdateSession>();
}