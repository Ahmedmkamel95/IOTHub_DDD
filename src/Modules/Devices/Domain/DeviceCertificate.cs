using CIOT.Common.Domain;

namespace CIOT.Modules.Devices.Domain;

public sealed class DeviceCertificate : EntityBase
{
    public Guid DeviceId { get; set; }
    public Device Device { get; set; } = null!;

    public string Thumbprint { get; set; } = null!;
    public string? SubjectName { get; set; }
    public DateTime ValidFromUtc { get; set; }
    public DateTime ValidToUtc { get; set; }
    public bool IsActive { get; set; } = true;
}
