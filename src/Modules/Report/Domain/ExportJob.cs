using CIOT.Common.Domain;

namespace CIOT.Modules.Report.Domain;

public sealed class ExportJob : AggregateRoot
{
    public string ExportType { get; set; } = "CSV"; // "CSV", "JSON", "Excel"
    public string Status { get; set; } = "Ready";
    public string? DownloadUrl { get; set; }
    public DateTime? ExpiresAtUtc { get; set; }
}
