using CIOT.Common.Domain;

namespace CIOT.Modules.Admin.Domain.Entities;

public sealed class VpmTargetConfiguration :
    BaseEntity<Guid>,
    IOptimisticConcurrentEntity,
    ICreationAuditableEntity,
    IUpdateAuditableEntity
{
    public int TargetYear { get; set; }

    public string CountryCode { get; set; } = default!;
    public string Category { get; set; } = default!;

    public string MachineType { get; set; } = default!;

    public string Manufacturer { get; set; } = default!;

    public string Model { get; set; } = default!;

    public Guid? EquipmentModelId { get; set; }
    public EquipmentModel? EquipmentModel { get; set; }

    public Guid? BusinessUnitId { get; set; }
    public string Status { get; set; } = default!;

    public long RowVersion { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }
    public Guid? CreatedByUserId { get; set; }

    public DateTimeOffset? UpdatedAtUtc { get; set; }
    public Guid? UpdatedByUserId { get; set; }
}