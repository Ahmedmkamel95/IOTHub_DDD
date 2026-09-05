namespace CIOT.Modules.Org.Application.Dtos;

public record CountryDto(string CountryCode, string CountryName, string? DefaultTimezone, bool IsActive);
public record CreateCountryRequest(string CountryCode, string CountryName, string? DefaultTimezone);

public record BusinessUnitDto(Guid Id, string BusinessUnitCode, string? BusinessUnitName, string CountryCode, bool IsActive);
public record CreateBusinessUnitRequest(string BusinessUnitCode, string? BusinessUnitName, string CountryCode);

public record SalesOrganizationDto(Guid Id, string SalesOrganizationCode, string DisplayName, string? CountryCode, bool IsActive);
public record CreateSalesOrganizationRequest(string SalesOrganizationCode, string DisplayName, string? CountryCode);
