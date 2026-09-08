using CIOT.Common.Domain;

namespace CIOT.Modules.Devices.Domain.Entities;

public sealed class FirmwareArtifactEquipmentModel : ICreationAuditableEntity
{
    public Guid FirmwareArtifactId { get; set; }
    public Guid EquipmentModelId { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public Guid? CreatedByUserId { get; set; }

    public FirmwareArtifact FirmwareArtifact { get; set; } = null!;
}