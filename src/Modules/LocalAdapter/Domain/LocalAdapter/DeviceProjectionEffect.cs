namespace CIOT.Modules.LocalAdapter.Domain.Entities;

public sealed class DeviceProjectionEffect
{
    public Guid OutboxId { get; set; }
    public Guid DeviceId { get; set; }
    public string CommandType { get; set; }
    public string ProjectionHash { get; set; }
    public string ProjectionJson { get; set; }
    public int ApplyCount { get; set; }
    public DateTimeOffset FirstAppliedAtUtc { get; set; }
    public DateTimeOffset LastAppliedAtUtc { get; set; }
}