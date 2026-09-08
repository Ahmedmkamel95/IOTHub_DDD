using CIOT.Common.Domain;

namespace CIOT.Modules.Org.Domain.Entities;

public sealed class SalesTerritory : BaseEntity<Guid>, ICreationAuditableEntity, IUpdateAuditableEntity
{
    public string TerritoryCode { get; set; } = null!;
    public string CountryCode { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public Guid? ParentTerritoryId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }

    public Country Country { get; set; } = null!;
    public SalesTerritory? ParentTerritory { get; set; }
}