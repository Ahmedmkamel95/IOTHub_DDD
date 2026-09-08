using CIOT.Common.Domain;

namespace CIOT.Modules.Report.Domain.Entities;

public sealed class ReportRun : BaseEntity<Guid>
{
    public string ReportCode { get; set; } = default!;

    public Guid? RequestedByUserId { get; set; }

    public string Status { get; set; } = default!;

    public string Format { get; set; } = default!;
    public string? Category { get; set; }
    public string? FilterJson { get; set; }
    public string? IdempotencyKey { get; set; }
    public string? PayloadHash { get; set; }
    public string? CorrelationId { get; set; }

    public DateTimeOffset RequestedAtUtc { get; set; }

    public DateTimeOffset? StartedAtUtc { get; set; }

    public DateTimeOffset? CompletedAtUtc { get; set; }
    public DateTimeOffset? NextAttemptAtUtc { get; set; }
    public string? LeaseOwner { get; set; }
    public DateTimeOffset? LeaseUntilUtc { get; set; }

    public string? OutputUri { get; set; }
    public string? OutputBlobName { get; set; }
    public string? OutputFileName { get; set; }
    public string? OutputContentType { get; set; }
    public long? OutputSizeBytes { get; set; }
    public string? OutputSha256 { get; set; }
    public DateTimeOffset? OutputExpiresAtUtc { get; set; }
    public int? ResultCount { get; set; }

    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTimeOffset? CancelledAtUtc { get; set; }
}