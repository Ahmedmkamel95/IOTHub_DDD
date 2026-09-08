using CIOT.Common.Domain;

namespace CIOT.Modules.Catalog.Domain.Entities;

public sealed class Material : BaseEntity<Guid>, ICreationAuditableEntity, IUpdateAuditableEntity, IOptimisticConcurrentEntity
{
    public string CountryCode { get; set; } = null!;
    public string MaterialCode { get; set; } = null!;
    public string? ProductBaseName { get; set; }
    public string? ProductName { get; set; }
    public string? Category { get; set; }
    public Guid? BusinessUnitId { get; set; }
    public bool IsActive { get; set; } = true;
    public string Status { get; set; } = default!;
    public string SourceSystem { get; set; } = default!;
    public string? SourcePayloadJson { get; set; }
    public long RowVersion { get; set; } = 1;
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
}