using CIOT.Common.Domain;

namespace CIOT.Modules.Admin.Domain.Entities;

public sealed class WaterFilterModel :
    BaseEntity<Guid>,
    ICreationActorAuditableEntity,
    IUpdateActorAuditableEntity,
    IOptimisticConcurrentEntity
{
    public string? Manufacturer { get; set; }

    public string Model { get; set; } = default!;

    public decimal CapacityL { get; set; }

    public decimal? WarningThresholdL { get; set; }

    public string? CountryCode { get; set; }
    public int? LifetimeMonths { get; set; }

    public decimal? WarningThresholdPercent { get; set; }

    public bool IsActive { get; set; } = true;

    public long RowVersion { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public Guid? CreatedByUserId { get; set; }

    public DateTimeOffset? UpdatedAtUtc { get; set; }
    public Guid? UpdatedByUserId { get; set; }
}