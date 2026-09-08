using CIOT.Common.Domain;

namespace CIOT.Modules.Admin.Domain.Entities;

public sealed class RecipeTemplate :
    BaseEntity<Guid>,
    ICreationAuditableEntity,
    IUpdateAuditableEntity,
    IOptimisticConcurrentEntity
{
    public string CountryCode { get; set; } = default!;

    public Guid EquipmentModelId { get; set; }
    public EquipmentModel EquipmentModel { get; set; } = default!;

    public Guid? RecipeConfigurationId { get; set; }
    public RecipeConfiguration? RecipeConfiguration { get; set; }

    public string RecipeCode { get; set; } = default!;

    public string RecipeName { get; set; } = default!;

    public string DosageJson { get; set; } = default!;

    public string Status { get; set; } = default!;

    public long RowVersion { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }
    public Guid? CreatedByUserId { get; set; }

    public DateTimeOffset? UpdatedAtUtc { get; set; }
    public Guid? UpdatedByUserId { get; set; }
}