using CIOT.Common.Domain;

namespace CIOT.Modules.Provisioning.Domain.Entities;

public sealed class DeviceFirmwareUpdate : BaseEntity<Guid>, ICreationAuditableEntity, IUpdateAuditableEntity
{
    public Guid ManufacturerDeviceId { get; set; }
    public Guid FirmwareArtifactId { get; set; }
    public string UpdateStatus { get; set; }
    public Guid RequestedByUserId { get; set; }
    public DateTimeOffset RequestedAtUtc { get; set; }
    public DateTimeOffset? CompletedAtUtc { get; set; }
    public string? FailureCode { get; set; }
    public string? FailureMessage { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
}