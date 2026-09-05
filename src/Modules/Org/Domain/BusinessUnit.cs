using CIOT.Common.Domain;

namespace CIOT.Modules.Org.Domain;

public sealed class BusinessUnit : AggregateRoot
{
    public string BusinessUnitCode { get; set; } = null!;
    public string? BusinessUnitName { get; set; }

    public string CountryCode { get; set; } = null!;
    public Country Country { get; set; } = null!;

    public bool IsActive { get; set; } = true;
}

