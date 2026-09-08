using CIOT.Modules.Devices.Application.Commands;
using CIOT.Modules.Devices.Application.Dtos;
using CIOT.Modules.Devices.Domain;
using CIOT.Unit.Tests.TestInfrastructure;
using Microsoft.EntityFrameworkCore;
using TUnit.Assertions;
using TUnit.Core;

namespace CIOT.Unit.Tests.Devices;

public sealed class DeviceTests
{
    [Test]
    public async Task RegisterDevice_NormalizesCountryAndUsesRegisteredStatus()
    {
        await using var context = TestDbContextFactory.CreateDevicesContext();
        var result = await new DeviceCommandHandlers(context).Handle(
            new RegisterDeviceCommand(new RegisterDeviceRequest("iot-001", "serial", null, null, "ae", "1.0")),
            CancellationToken.None
        );

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.Value.CountryCode).IsEqualTo("AE");
        await Assert.That(result.Value.LifecycleStatus).IsEqualTo("Registered");
        await Assert.That(await context.Devices.CountAsync()).IsEqualTo(1);
    }

    [Test]
    public async Task UpdateDeviceStatus_ActivatesExistingDevice()
    {
        await using var context = TestDbContextFactory.CreateDevicesContext();
        var device = new Device { Id = Guid.NewGuid(), IotHubDeviceId = "iot-002" };
        context.Devices.Add(device);
        await context.SaveChangesAsync();

        var result = await new DeviceCommandHandlers(context).Handle(
            new UpdateDeviceStatusCommand(device.Id, "active"),
            CancellationToken.None
        );

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That((await context.Devices.FindAsync(device.Id))!.LifecycleStatus).IsEqualTo("Active");
    }

    [Test]
    public async Task Device_RecordHeartbeat_SetsFirstAndLastSeenAndFirmware()
    {
        var device = new Device();

        device.RecordHeartbeat("2.0");

        await Assert.That(device.FirstSeenAtUtc).IsNotNull();
        await Assert.That(device.LastSeenAtUtc).IsNotNull();
        await Assert.That(device.FirmwareVersion).IsEqualTo("2.0");
    }

    [Test]
    public async Task RegisterDeviceValidator_RejectsMissingHubId()
    {
        var result = await new RegisterDeviceCommandValidator().ValidateAsync(
            new RegisterDeviceCommand(new RegisterDeviceRequest(string.Empty, null, null, null, null, null))
        );

        await Assert.That(result.IsValid).IsFalse();
    }
}
