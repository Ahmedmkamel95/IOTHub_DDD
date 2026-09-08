using CIOT.Common.Domain;

namespace CIOT.Modules.Admin.Domain.Entities;

public sealed class AssetTargetDefault : BaseEntity<Guid>, IEffectiveDatingEntity, ICreationActorAuditableEntity
{
    public string CountryCode { get; set; } = default!;

    public Guid? EquipmentModelId { get; set; }
    public EquipmentModel? EquipmentModel { get; set; } 

    public string? AssetTypeCode { get; set; }
    
    public string MetricKey { get; set; } = default!;
    public decimal TargetValue { get; set; }
    public string Unit { get; set; } = default!;

    public DateTimeOffset ValidFromUtc { get; set; }
    public DateTimeOffset? ValidToUtc { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public Guid? CreatedByUserId { get; set; } 
}