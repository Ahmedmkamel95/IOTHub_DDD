using CIOT.Common.Domain;

namespace CIOT.Modules.Org.Domain.Entities;

public sealed class BusinessUnit : BaseEntity<Guid>, ICreationAuditableEntity, IUpdateAuditableEntity
{
    public string BusinessUnitCode { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }

    public ICollection<BusinessUnitCountry> BusinessUnitCountries { get; } = new List<BusinessUnitCountry>();
}