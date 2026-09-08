using CIOT.Common.Domain;

namespace CIOT.Modules.Admin.Domain.Entities;

public sealed class DeviceModelControlCatalog : BaseEntity<Guid>,
ICreationAuditableEntity,
IUpdateAuditableEntity
{
    public Guid EquipmentModelId { get; set; }
    public EquipmentModel EquipmentModel { get; set; } = default!;

    public string ControlKey { get; set; } = default!;
    public string ControlKind { get; set; } = default!;

    public int ContractVersion { get; set; }

    public string JsonSchema { get; set; } = default!;

    public string? MinimumFirmwareVersion { get; set; }

    public string RequiredPermissionCode { get; set; } = default!;

    public string RiskLevel { get; set; } = default!;

    public int? TimeoutSeconds { get; set; }

    public bool IsIdempotent { get; set; }

    public string Status { get; set; } = default!;

    public DateTimeOffset CreatedAtUtc { get; set; }
    public Guid? CreatedByUserId { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
    public Guid? UpdatedByUserId { get; set; }
}