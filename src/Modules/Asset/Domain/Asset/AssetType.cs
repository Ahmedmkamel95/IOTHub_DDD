using CIOT.Common.Domain;

namespace CIOT.Modules.Asset.Domain.Entities;

public sealed class AssetType : ICreationAuditableEntity
{
    public string AssetTypeCode { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string? MachineType { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
}