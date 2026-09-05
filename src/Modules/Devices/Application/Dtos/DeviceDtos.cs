namespace CIOT.Modules.Devices.Application.Dtos;

public record DeviceDto(
    Guid Id,
    string IotHubDeviceId,
    string? DeviceSerialNumber,
    string? Imei,
    string? MacAddress,
    string? CountryCode,
    string LifecycleStatus,
    string? FirmwareVersion,
    DateTime? FirstSeenAtUtc,
    DateTime? LastSeenAtUtc,
    Guid? CurrentPairedAssetId
);

public record RegisterDeviceRequest(
    string IotHubDeviceId,
    string? DeviceSerialNumber,
    string? Imei,
    string? MacAddress,
    string? CountryCode,
    string? FirmwareVersion
);

public record UpdateDeviceStatusRequest(string LifecycleStatus);

public record DispatchCommandRequest(
    string CommandType,
    string PayloadJson,
    string DeliveryPath = "C2D"
);

public record DeviceCommandDto(
    Guid Id,
    Guid DeviceId,
    string CommandType,
    string PayloadJson,
    string DeliveryPath,
    string Status,
    DateTime? EnqueuedAtUtc,
    DateTime? CompletedAtUtc,
    string? LastError
);

public record PairDeviceRequest(Guid AssetId, string AssignmentType = "Production");
