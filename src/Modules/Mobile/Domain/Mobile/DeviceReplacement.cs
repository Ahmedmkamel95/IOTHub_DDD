using CIOT.Common.Domain;

namespace CIOT.Modules.Mobile.Domain.Entities;

public sealed class DeviceReplacement : BaseEntity<Guid>, ICreationAuditableEntity
{
    public Guid AssetId { get; set; }
    public Guid PreviousDeviceId { get; set; }
    public Guid ReplacementDeviceId { get; set; }
    public Guid? PreviousAssignmentId { get; set; }
    public Guid? ReplacementAssignmentId { get; set; }
    public Guid ChangedByUserId { get; set; }
    public DateTimeOffset ChangedAtUtc { get; set; }
    public bool RetainWaterFilter { get; set; }
    public bool TransferDosageSettings { get; set; }
    public Guid? RequestedFirmwareVersionId { get; set; }
    public string Status { get; set; }
    public string CorrelationId { get; set; }
    public string? EvidenceJson { get; set; }
    public string? Notes { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
}