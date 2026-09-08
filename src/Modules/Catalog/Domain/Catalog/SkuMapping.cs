using CIOT.Common.Domain;

namespace CIOT.Modules.Catalog.Domain.Entities;

public sealed class SkuMapping : BaseEntity<Guid>, ICreationAuditableEntity, IUpdateAuditableEntity, IOptimisticConcurrentEntity
{
    public string SkuCode { get; set; } = default!;
    public Guid MaterialId { get; set; }
    public Material Material { get; set; } = null!;

    public Guid? EquipmentModelId { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid? BusinessUnitId { get; set; }

    public string MappingStatus { get; set; } = default!;

    public string? MachineManufacturer { get; set; }
    public string? MachineModel { get; set; }
    public string? MachineSubmodel { get; set; }
    public string? MachineType { get; set; }

    public string? TelemetryProductCode { get; set; }
    public string? AttributesJson { get; set; }
    public long RowVersion { get; set; } = 1;

    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
}