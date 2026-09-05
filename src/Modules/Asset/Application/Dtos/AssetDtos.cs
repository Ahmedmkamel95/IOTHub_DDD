namespace CIOT.Modules.Asset.Application.Dtos;

public record AssetDto(
    Guid Id,
    string SapEquipmentNumber,
    string? OemSerialNumber,
    string? TechnicalId,
    Guid? EquipmentModelId,
    string CountryCode,
    string? SapStatus,
    bool IsActive,
    Guid? CurrentOutletId,
    DateTime? LastConnectionAtUtc
);

public record RegisterAssetRequest(
    string SapEquipmentNumber,
    string? OemSerialNumber,
    string? TechnicalId,
    Guid? EquipmentModelId,
    string CountryCode,
    string? SapStatus
);

public record AssignAssetToOutletRequest(Guid OutletId, Guid? CustomerId = null);

public record AssignAssetToCustomerOutletRequest(Guid CustomerId, Guid? OutletId = null, Guid? ClusterId = null);

public record AssetOutletAssignmentDto(
    Guid Id,
    Guid AssetId,
    Guid? OutletId,
    Guid? CustomerId,
    DateTime AssignedAtUtc,
    DateTime? UnassignedAtUtc,
    bool IsCurrent
);
