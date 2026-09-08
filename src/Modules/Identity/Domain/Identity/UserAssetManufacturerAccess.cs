using CIOT.Common.Domain;

namespace CIOT.Modules.Identity.Domain.Entities;

public sealed class UserAssetManufacturerAccess : IEffectiveDatingEntity, ICreationAuditableEntity
{
    public Guid UserId { get; set; }
    public Guid AssetManufacturerId { get; set; }
    public DateTimeOffset ValidFromUtc { get; set; }
    public DateTimeOffset? ValidToUtc { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
}