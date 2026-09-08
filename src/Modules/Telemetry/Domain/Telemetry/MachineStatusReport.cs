using CIOT.Common.Domain;

namespace CIOT.Modules.Telemetry.Domain.Entities;

public sealed class MachineStatusReport : BaseEntity<Guid>, ICreationAuditableEntity
{
    public DateTimeOffset ReportedAtUtc { get; set; }

    public Guid? RawMessageId { get; set; }

    public Guid? NormalizedEventId { get; set; }

    public DateTimeOffset? NormalizedEventAtUtc { get; set; }

    public Guid? NormalizedMeasurementId { get; set; }

    public DateTimeOffset? NormalizedObservedAtUtc { get; set; }

    public Guid? DeviceId { get; set; }

    public Guid AssetId { get; set; }

    public Guid? OutletId { get; set; }

    public Guid? CustomerId { get; set; }

    public string? CountryCode { get; set; }

    public string? MachineType { get; set; }

    public string Status { get; set; } = default!;

    public string? ConnectionStatus { get; set; }

    public string? MachineMode { get; set; }

    public bool? IsOnline { get; set; }

    public DateTimeOffset? LastCommunicationAtUtc { get; set; }

    public string? StatusPayloadJson { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }
}