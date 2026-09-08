using CIOT.Common.Domain;

namespace CIOT.Modules.Devices.Domain.Entities;

public sealed class FirmwareArtifactDeviceModel : ICreationAuditableEntity
{
    public Guid FirmwareArtifactId { get; set; }
    public Guid DeviceModelId { get; set; }
    public string CompatibilityStatus { get; set; }
    public string? MinCurrentFirmwareVersion { get; set; }
    public string? MaxCurrentFirmwareVersion { get; set; }
    public string? Notes { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public Guid? CreatedByUserId { get; set; }

    public FirmwareArtifact FirmwareArtifact { get; set; } = null!;
}