using CIOT.Common.Domain;

namespace CIOT.Modules.Asset.Domain.Entities;

public sealed class AssetIdentifier : BaseEntity<Guid>, ICreationAuditableEntity
{
    public Guid AssetId { get; set; }
    public string IdentifierType { get; set; } = null!;
    public string IdentifierValue { get; set; } = null!;
    public bool IsPrimary { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
}