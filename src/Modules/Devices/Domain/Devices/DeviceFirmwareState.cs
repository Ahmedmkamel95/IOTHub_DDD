using CIOT.Common.Domain;

namespace CIOT.Modules.Devices.Domain.Entities;

public sealed class DeviceFirmwareState : IUpdateAuditableEntity
{
    public Guid DeviceId { get; set; }
    public string? CurrentFirmwareVersion { get; set; }
    public string? TargetFirmwareVersion { get; set; }
    public string? UpdateStatus { get; set; }
    public int? UpdateProgressPercent { get; set; }
    public DateTimeOffset? LastManifestCheckAtUtc { get; set; }
    public DateTimeOffset? LastStatusAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }

    public Device Device { get; set; } = null!;
}