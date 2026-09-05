using CIOT.Common.Domain;

namespace CIOT.Modules.Report.Domain;

public sealed class ReportRun : EntityBase
{
    public Guid ReportDefinitionId { get; set; }
    public Guid RequestedByUserId { get; set; }
    public string Status { get; set; } = "Completed"; // "Queued", "Running", "Completed", "Failed"
    public string? ResultUri { get; set; }
    public DateTime StartedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAtUtc { get; set; }
}
