using CIOT.Common.Domain;

namespace CIOT.Modules.Org.Domain.Entities;

public sealed class BusinessUnitCountry : ICreationAuditableEntity
{
    public Guid BusinessUnitId { get; set; }
    public string CountryCode { get; set; } = null!;
    public DateTimeOffset CreatedAtUtc { get; set; }
}