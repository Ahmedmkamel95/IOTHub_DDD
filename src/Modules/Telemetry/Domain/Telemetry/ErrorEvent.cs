using CIOT.Common.Domain;

namespace CIOT.Modules.Telemetry.Domain.Entities;

public sealed class ErrorEvent : BaseEntity<Guid>, ICreationAuditableEntity
{
    public DateTimeOffset EventAtUtc { get; set; }

    public Guid? RawMessageId { get; set; }

    public Guid? NormalizedEventId { get; set; }

    public DateTimeOffset? NormalizedEventAtUtc { get; set; }

    public Guid? DeviceId { get; set; }

    public Guid? AssetId { get; set; }

    public Guid? OutletId { get; set; }

    public Guid? CustomerId { get; set; }

    public string? CountryCode { get; set; }

    public string? MachineType { get; set; }

    public string SourceErrorCode { get; set; } = default!;

    public Guid? MappedErrorId { get; set; }

    public string? Severity { get; set; }

    public string? ErrorState { get; set; }

    public string? Message { get; set; }

    public string? ErrorPayloadJson { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }
}