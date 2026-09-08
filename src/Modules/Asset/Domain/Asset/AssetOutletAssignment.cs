using CIOT.Common.Domain;

namespace CIOT.Modules.Asset.Domain.Entities;

public sealed class AssetOutletAssignment : BaseEntity<Guid>, ICreationAuditableEntity
{
    public string? SourceSystem { get; set; } = default!;
    public Guid AssetId { get; set; }

    public Guid? OutletId { get; set; }

    public Guid? CustomerId { get; set; }

    public DateTimeOffset AssignedAtUtc { get; set; }

    public DateTimeOffset? RemovedAtUtc { get; set; }

    public Guid? AssignedByUserId { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }
}