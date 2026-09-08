using CIOT.Common.Domain;

namespace CIOT.Modules.Integration.Domain.Entities;

public sealed class ImportBatchItem : BaseEntity<Guid>, ICreationAuditableEntity
{
    public Guid ImportBatchId { get; set; }
    public int ItemIndex { get; set; }
    public string? SourceEntityCode { get; set; }
    public string? TargetTable { get; set; }
    public Guid? TargetRecordId { get; set; }
    public string Status { get; set; } = null!;
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public string? SourcePayloadJson { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
}