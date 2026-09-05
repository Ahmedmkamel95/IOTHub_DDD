namespace CIOT.Modules.Provisioning.Application.Dtos;

public record DeviceManufacturerDto(Guid Id, string ManufacturerCode, string DisplayName, string Status);
public record CreateDeviceManufacturerRequest(string ManufacturerCode, string DisplayName);

public record DeviceModelDto(Guid Id, Guid DeviceManufacturerId, string ModelCode, string DisplayName, string? HardwareRevision, string Status);
public record CreateDeviceModelRequest(Guid DeviceManufacturerId, string ModelCode, string DisplayName, string? HardwareRevision);
