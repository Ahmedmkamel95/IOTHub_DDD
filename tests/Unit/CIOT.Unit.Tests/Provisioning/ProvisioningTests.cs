using CIOT.Modules.Provisioning.Application.Commands;
using CIOT.Modules.Provisioning.Application.Dtos;
using CIOT.Modules.Provisioning.Domain;
using CIOT.Unit.Tests.TestInfrastructure;
using Microsoft.EntityFrameworkCore;
using TUnit.Assertions;
using TUnit.Core;

namespace CIOT.Unit.Tests.Provisioning;

public sealed class ProvisioningTests
{
    [Test]
    public async Task CreateDeviceManufacturer_NormalizesCodeAndSetsActiveStatus()
    {
        await using var context = TestDbContextFactory.CreateProvisioningContext();
        var result = await new ProvisioningCommandHandlers(context).Handle(
            new CreateDeviceManufacturerCommand(new CreateDeviceManufacturerRequest("acme", "Acme")),
            CancellationToken.None
        );

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.Value.ManufacturerCode).IsEqualTo("ACME");
        await Assert.That(result.Value.Status).IsEqualTo("Active");
        await Assert.That(await context.DeviceManufacturers.CountAsync()).IsEqualTo(1);
    }

    [Test]
    public async Task CreateDeviceModel_WhenManufacturerMissing_ReturnsNotFound()
    {
        await using var context = TestDbContextFactory.CreateProvisioningContext();
        var result = await new ProvisioningCommandHandlers(context).Handle(
            new CreateDeviceModelCommand(new CreateDeviceModelRequest(Guid.NewGuid(), "model-1", "Model 1", null)),
            CancellationToken.None
        );

        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Error.Code).IsEqualTo("Manufacturer.NotFound");
    }

    [Test]
    public async Task CreateDeviceModel_WhenManufacturerExists_NormalizesModelCode()
    {
        await using var context = TestDbContextFactory.CreateProvisioningContext();
        var manufacturer = new DeviceManufacturer
        {
            Id = Guid.NewGuid(),
            ManufacturerCode = "ACME",
            DisplayName = "Acme"
        };
        context.DeviceManufacturers.Add(manufacturer);
        await context.SaveChangesAsync();

        var result = await new ProvisioningCommandHandlers(context).Handle(
            new CreateDeviceModelCommand(new CreateDeviceModelRequest(manufacturer.Id, "model-1", "Model 1", "A")),
            CancellationToken.None
        );

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.Value.ModelCode).IsEqualTo("MODEL-1");
    }
}
