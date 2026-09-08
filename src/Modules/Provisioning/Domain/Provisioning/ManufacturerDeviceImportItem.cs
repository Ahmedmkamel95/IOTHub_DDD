using CIOT.Common.Domain;

namespace CIOT.Modules.Provisioning.Domain.Entities;

public sealed class ManufacturerDeviceImportItem : BaseEntity<Guid>, ICreationAuditableEntity
{
    public Guid ImportBatchId { get; set; }
    public int RowNumber { get; set; }
    public Guid? ManufacturerDeviceId { get; set; }
    public string Status { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
}