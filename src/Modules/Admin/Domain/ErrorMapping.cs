using CIOT.Common.Domain;

namespace CIOT.Modules.Admin.Domain;

public sealed class ErrorMapping : EntityBase
{
    public string Manufacturer { get; set; } = null!;
    public string RawErrorCode { get; set; } = null!;
    public string StandardErrorCode { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Severity { get; set; } = "Warning";
}
