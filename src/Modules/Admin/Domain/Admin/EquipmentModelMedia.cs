using CIOT.Common.Domain;

namespace CIOT.Modules.Admin.Domain.Entities;

public sealed class EquipmentModelMedia : BaseEntity<Guid>,
    ICreationActorAuditableEntity,
    IUpdateActorAuditableEntity
{
    public Guid EquipmentModelId { get; set; }
    public EquipmentModel EquipmentModel { get; set; } = default!;

    public string MediaKind { get; set; } = default!;

    public string ObjectStorageUri { get; set; } = default!;

    public string ContentType { get; set; } = default!;

    public string ContentSha256 { get; set; } = default!;

    public string MalwareScanStatus { get; set; } = default!;

    public string Status { get; set; } = default!;

    public DateTimeOffset CreatedAtUtc { get; set; }
    public Guid? CreatedByUserId { get; set; }

    public DateTimeOffset? UpdatedAtUtc { get; set; }
    public Guid? UpdatedByUserId { get; set; }
}