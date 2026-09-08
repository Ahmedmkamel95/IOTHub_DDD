using CIOT.Common.Domain;

namespace CIOT.Modules.Telemetry.Domain.Entities;

public sealed class LocationResolutionOutbox : BaseEntity<Guid>, ICreationAuditableEntity, IUpdateAuditableEntity
{
    public Guid ConnectivityReportId { get; set; }
    public DateTimeOffset ConnectivityReportedAtUtc { get; set; }
    public Guid DeviceId { get; set; }
    public string Provider { get; set; }
    public string RequestHash { get; set; }
    public int AttemptNumber { get; set; }
    public string CorrelationId { get; set; }
    public string PayloadJson { get; set; }
    public string Status { get; set; }
    public int PublishAttemptCount { get; set; }
    public DateTimeOffset NextAttemptAtUtc { get; set; }
    public string? LockedBy { get; set; }
    public DateTimeOffset? LockedUntilUtc { get; set; }
    public DateTimeOffset? PublishedAtUtc { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }

    public ConnectivityReport? ConnectivityReport { get; set; }
}