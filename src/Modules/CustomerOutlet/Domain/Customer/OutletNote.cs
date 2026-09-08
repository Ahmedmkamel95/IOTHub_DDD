using CIOT.Common.Domain;

namespace CIOT.Modules.CustomerOutlet.Domain.Entities;

public sealed class OutletNote : BaseEntity<Guid>, ICreationActorAuditableEntity, IUpdateActorAuditableEntity
{
    public Guid OutletId { get; set; }
    public Guid? RelatedEquipmentId { get; set; }
    public Guid? RelatedAssetId { get; set; }

    public string NoteText { get; set; } = default!;
    public string NoteBody { get; set; } = default!;
    public bool IsDeleted { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }
    public Guid? CreatedByUserId { get; set; }

    public DateTimeOffset? UpdatedAtUtc { get; set; }
    public Guid? UpdatedByUserId { get; set; }

    public DateTimeOffset? DeletedAtUtc { get; set; }
}