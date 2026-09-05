using CIOT.Common.Domain;

namespace CIOT.Modules.Report.Domain;

public sealed class ReportDefinition : AggregateRoot
{
    public string ReportCode { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string? Description { get; set; }
    public string ReportType { get; set; } = "TelemetrySummary"; // "TelemetrySummary", "AssetStatus", "Consumption"
    public string? ConfigurationJson { get; set; }
    public bool IsActive { get; set; } = true;
}
