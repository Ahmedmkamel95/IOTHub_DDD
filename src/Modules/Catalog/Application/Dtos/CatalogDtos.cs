namespace CIOT.Modules.Catalog.Application.Dtos;

public record MaterialDto(Guid Id, string MaterialCode, string? ProductName, string CountryCode, bool IsActive);
public record CreateMaterialRequest(string MaterialCode, string? ProductName, string CountryCode, Guid? BusinessUnitId = null);
