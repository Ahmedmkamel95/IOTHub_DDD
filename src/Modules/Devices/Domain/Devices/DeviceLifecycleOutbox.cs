using CIOT.Common.Domain;
using DeviceEntity = CIOT.Modules.Devices.Domain.Entities.Device;

namespace CIOT.Modules.Devices.Domain.Entities;

public sealed class DeviceLifecycleOutbox : BaseEntity<Guid>, ICreationAuditableEntity
{
    public Guid DeviceId { get; set; }

    public string CommandType { get; set; } = default!;

    public string PayloadJson { get; set; } = default!;

    public string Status { get; set; } = default!;

    public int RetryCount { get; set; }

    public DateTimeOffset? NextAttemptAtUtc { get; set; }
    public string? LockedBy { get; set; }
    public DateTimeOffset? LockedUntilUtc { get; set; }
    public string? CorrelationId { get; set; }
    public string? LastProjectionHash { get; set; }
    public string? IdempotencyKey { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }

    public DateTimeOffset? ProcessedAtUtc { get; set; }

    public string? ErrorMessage { get; set; }

    public DeviceEntity? Device { get; set; }
}