using CIOT.Common.Domain;

namespace CIOT.Modules.Report.Domain.Entities;

public sealed class ReportDefinition : BaseEntity<Guid>, ICreationAuditableEntity, IUpdateAuditableEntity
{
    public string ReportCode { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? SourceCode { get; set; }
    public string? RequiredPermissionCode { get; set; }
    public string DefaultFormat { get; set; } = default!;
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
}