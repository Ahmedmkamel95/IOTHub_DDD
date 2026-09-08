using CIOT.Common.Domain;

namespace CIOT.Modules.Telemetry.Domain.Entities;

public sealed class LocalLocationResolutionQueue : ICreationAuditableEntity, IUpdateAuditableEntity
{
    public Guid MessageId { get; set; }
    public Guid ConnectivityReportId { get; set; }
    public DateTimeOffset ConnectivityReportedAtUtc { get; set; }
    public Guid DeviceId { get; set; }
    public string Provider { get; set; }
    public string RequestHash { get; set; }
    public int AttemptNumber { get; set; }
    public string CorrelationId { get; set; }
    public string PayloadJson { get; set; }
    public string Status { get; set; }
    public DateTimeOffset ScheduledAtUtc { get; set; }
    public int DeliveryCount { get; set; }
    public string? LeaseOwner { get; set; }
    public DateTimeOffset? LeaseUntilUtc { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTimeOffset? CompletedAtUtc { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
    public ConnectivityReport? ConnectivityReport { get; set; }
}