using CIOT.Modules.Org.Application.Commands;
using CIOT.Modules.Org.Application.Dtos;
using CIOT.Unit.Tests.TestInfrastructure;
using Microsoft.EntityFrameworkCore;
using TUnit.Assertions;
using TUnit.Core;

namespace CIOT.Unit.Tests.Org;

public sealed class OrgCommandHandlerTests
{
    [Test]
    public async Task CreateCountry_NormalizesCodeAndPersistsCountry()
    {
        await using var context = TestDbContextFactory.CreateOrgContext();
        var handler = new OrgCommandHandlers(context);

        var result = await handler.Handle(
            new CreateCountryCommand(new CreateCountryRequest(" ae ", "United Arab Emirates", "Asia/Dubai")),
            CancellationToken.None
        );

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.Value.CountryCode).IsEqualTo(" AE ");
        await Assert.That(await context.Countries.CountAsync()).IsEqualTo(1);
    }

    [Test]
    public async Task CreateCountry_WhenCodeExists_ReturnsConflict()
    {
        await using var context = TestDbContextFactory.CreateOrgContext();
        context.Countries.Add(new() { CountryCode = "AE", CountryName = "United Arab Emirates" });
        await context.SaveChangesAsync();
        var handler = new OrgCommandHandlers(context);

        var result = await handler.Handle(
            new CreateCountryCommand(new CreateCountryRequest("ae", "Duplicate", null)),
            CancellationToken.None
        );

        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Error.Code).IsEqualTo("Country.Duplicate");
    }

    [Test]
    public async Task CreateBusinessUnit_WhenCountryDoesNotExist_ReturnsNotFound()
    {
        await using var context = TestDbContextFactory.CreateOrgContext();
        var handler = new OrgCommandHandlers(context);

        var result = await handler.Handle(
            new CreateBusinessUnitCommand(new CreateBusinessUnitRequest("BU1", "Operations", "AE")),
            CancellationToken.None
        );

        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Error.Code).IsEqualTo("Country.NotFound");
    }

    [Test]
    public async Task CreateBusinessUnit_WhenCountryExists_PersistsNormalizedValues()
    {
        await using var context = TestDbContextFactory.CreateOrgContext();
        context.Countries.Add(new() { CountryCode = "AE", CountryName = "United Arab Emirates" });
        await context.SaveChangesAsync();
        var handler = new OrgCommandHandlers(context);

        var result = await handler.Handle(
            new CreateBusinessUnitCommand(new CreateBusinessUnitRequest("bu1", "Operations", "ae")),
            CancellationToken.None
        );

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.Value.BusinessUnitCode).IsEqualTo("BU1");
        await Assert.That(result.Value.CountryCode).IsEqualTo("AE");
    }
}
