using CIOT.Common.Domain;

namespace CIOT.Modules.Admin.Domain.Entities;

public sealed class OperationalStatusPolicy :
    IOptimisticConcurrentEntity,
    ICreationActorAuditableEntity,
    IUpdateActorAuditableEntity
{
    public string PolicyCode { get; set; } = default!;

    public int OnlineMaxAgeMinutes { get; set; }

    public int OfflineMinAgeMinutes { get; set; }

    public bool ExplicitOfflineIsAuthoritative { get; set; }

    public bool DecommissionedIsAuthoritative { get; set; }

    public string Status { get; set; } = default!;

    public long RowVersion { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }
    public Guid? CreatedByUserId { get; set; }

    public DateTimeOffset? UpdatedAtUtc { get; set; }
    public Guid? UpdatedByUserId { get; set; }
}