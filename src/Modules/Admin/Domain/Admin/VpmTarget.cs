using CIOT.Common.Domain;

namespace CIOT.Modules.Admin.Domain.Entities;

public sealed class VpmTarget :
    BaseEntity<Guid>,
    ICreationAuditableEntity,
    IUpdateAuditableEntity
{
    public Guid? TargetConfigurationId { get; set; }
    public VpmTargetConfiguration? TargetConfiguration { get; set; }

    public string CountryCode { get; set; } = default!;

    public Guid? EquipmentModelId { get; set; }
    public EquipmentModel? EquipmentModel { get; set; }

    public string? MachineType { get; set; }

    public int TargetYear { get; set; }

    public int? TargetMonth { get; set; }

    public string MetricKey { get; set; } = default!;

    public decimal TargetValue { get; set; }

    public string Unit { get; set; } = default!;

    public DateTimeOffset CreatedAtUtc { get; set; }
    public Guid? CreatedByUserId { get; set; }

    public DateTimeOffset? UpdatedAtUtc { get; set; }
    public Guid? UpdatedByUserId { get; set; }
}