using CIOT.Common.Domain;

namespace CIOT.Modules.Org.Domain;

public sealed class BusinessUnitCountry : ValueObject
{
    public Guid BusinessUnitId { get; set; }
    public string CountryCode { get; set; } = null!;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return BusinessUnitId;
        yield return CountryCode;
    }
}
