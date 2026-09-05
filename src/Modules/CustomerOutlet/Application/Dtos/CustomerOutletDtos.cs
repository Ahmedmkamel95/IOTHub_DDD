namespace CIOT.Modules.CustomerOutlet.Application.Dtos;

public record CustomerClusterDto(
    Guid Id,
    string ClusterCode,
    string ClusterName,
    string? Description,
    bool IsActive
);

public record CreateCustomerClusterRequest(
    string ClusterCode,
    string ClusterName,
    string? Description = null,
    bool IsActive = true
);

public record CustomerDto(
    Guid Id,
    string CustomerCode,
    string? CustomerName1,
    string? CustomerName2,
    string CountryCode,
    string? VatNumber,
    bool IsActive,
    Guid? CustomerClusterId = null
);

public record CreateCustomerRequest(
    string CustomerCode,
    string? CustomerName1,
    string? CustomerName2,
    string CountryCode,
    string? VatNumber,
    string? WholesalerCode,
    Guid? CustomerClusterId = null
);

public record OutletDto(
    Guid Id,
    string OutletCode,
    Guid? CustomerId,
    string? OutletType,
    string? AddressLine,
    string? City,
    string? PostalCode,
    string CountryCode,
    decimal? Latitude,
    decimal? Longitude,
    Guid? SalesTerritoryId,
    bool IsActive
);

public record CreateOutletRequest(
    string OutletCode,
    Guid? CustomerId,
    string? OutletType,
    string? AddressLine,
    string? City,
    string? PostalCode,
    string CountryCode,
    decimal? Latitude,
    decimal? Longitude,
    Guid? SalesTerritoryId
);

public record OutletNoteDto(Guid Id, Guid OutletId, Guid? RelatedAssetId, string NoteBody, DateTime CreatedAtUtc);
public record AddOutletNoteRequest(string NoteBody, Guid? RelatedAssetId = null);
