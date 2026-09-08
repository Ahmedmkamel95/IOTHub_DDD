using CIOT.Common.Domain;

namespace CIOT.Modules.Devices.Domain.Entities;

public sealed class DeviceCommand : BaseEntity<Guid>, ICreationAuditableEntity
{
    public Guid DeviceId { get; set; }
    public Device Device { get; set; } = null!;

    public Guid? BulkActionId { get; set; }
    public string CommandType { get; set; } = null!;
    public string CommandStatus { get; set; } = default!;

    public Guid? RequestedByUserId { get; set; }
    public DateTimeOffset RequestedAtUtc { get; set; }

    public DateTimeOffset? SentAtUtc { get; set; }
    public DateTimeOffset? CompletedAtUtc { get; set; }

    public string? RequestPayloadJson { get; set; }
    public string? ResponsePayloadJson { get; set; }

    public string? CorrelationId { get; set; }
    public string? Notes { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
}