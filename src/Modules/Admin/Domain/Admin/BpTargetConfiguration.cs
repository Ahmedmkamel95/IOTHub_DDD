using CIOT.Common.Domain;

namespace CIOT.Modules.Admin.Domain.Entities;

public sealed class BpTargetConfiguration : BaseEntity<Guid>, IOptimisticConcurrentEntity , ICreationActorAuditableEntity , IUpdateActorAuditableEntity
{
    public int TargetYear { get; set; }
    public Guid BusinessUnitId { get; set; }
    
    public string CountryCode { get; set; } = default!;
    
    public string Category { get; set; } = default!;
    public decimal VpmKgPerMachine { get; set; }
    public string Status { get; set; } = default!;
    public long RowVersion { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public Guid? CreatedByUserId { get; set; }

    public DateTimeOffset? UpdatedAtUtc { get; set; }
    public Guid? UpdatedByUserId { get; set; }

}