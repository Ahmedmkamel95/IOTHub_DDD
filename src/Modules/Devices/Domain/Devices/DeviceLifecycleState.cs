using CIOT.Common.Domain;

namespace CIOT.Modules.Devices.Domain.Entities;

public sealed class DeviceLifecycleState : IUpdateAuditableEntity
{
    public Guid DeviceId { get; set; }
    public bool DesiredEnabled { get; set; }
    public bool? IotHubEnabled { get; set; }
    public string? DesiredTagsJson { get; set; }
    public string? ReportedTagsJson { get; set; }
    public string? DesiredPropertiesJson { get; set; }
    public string? ReportedPropertiesJson { get; set; }
    public DateTimeOffset? LastProjectedAtUtc { get; set; }
    public long LifecycleVersion { get; set; } = 1;
    public DateTimeOffset? UpdatedAtUtc { get; set; }

    public Device Device { get; set; } = null!;
}