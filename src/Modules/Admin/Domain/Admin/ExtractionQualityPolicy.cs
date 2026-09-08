using CIOT.Common.Domain;

namespace CIOT.Modules.Admin.Domain.Entities;

public sealed class ExtractionQualityPolicy : BaseEntity<Guid>,
    IOptimisticConcurrentEntity,
    IEffectiveDatingEntity,
    ICreationAuditableEntity,
    IUpdateAuditableEntity
{
    public Guid? EquipmentModelId { get; set; }
    public EquipmentModel? EquipmentModel { get; set; }

    public string? CountryCode { get; set; }
    public string? RecipeCode { get; set; }

    public string MeasurementCode { get; set; } = default!;
    public string MeasurementUnit { get; set; } = default!;

    public decimal LowerInclusive { get; set; }
    public decimal UpperInclusive { get; set; }

    public int Priority { get; set; }

    public string Status { get; set; } = default!;

    public DateTimeOffset ValidFromUtc { get; set; }
    public DateTimeOffset? ValidToUtc { get; set; }

    public long RowVersion { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }
    public Guid? CreatedByUserId { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
    public Guid? UpdatedByUserId { get; set; }
}