using CIOT.Modules.Org.Application.Queries;
using CIOT.Modules.Org.Domain;
using CIOT.Unit.Tests.TestInfrastructure;
using TUnit.Assertions;
using TUnit.Core;

namespace CIOT.Unit.Tests.Org;

public sealed class OrgQueryHandlerTests
{
    [Test]
    public async Task GetCountries_DefaultsToActiveAndSortsByName()
    {
        await using var context = TestDbContextFactory.CreateOrgContext();
        context.Countries.AddRange(
            new Country { CountryCode = "US", CountryName = "United States", IsActive = true },
            new Country { CountryCode = "AE", CountryName = "United Arab Emirates", IsActive = true },
            new Country { CountryCode = "EG", CountryName = "Egypt", IsActive = false }
        );
        await context.SaveChangesAsync();

        var result = await new OrgQueryHandlers(context).Handle(new GetCountriesQuery(), CancellationToken.None);

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.Value.Select(country => country.CountryCode).ToArray())
            .IsEquivalentTo(["AE", "US"]);
    }

    [Test]
    public async Task GetCountries_WhenActiveOnlyFalseIncludesInactiveCountries()
    {
        await using var context = TestDbContextFactory.CreateOrgContext();
        context.Countries.Add(new Country { CountryCode = "EG", CountryName = "Egypt", IsActive = false });
        await context.SaveChangesAsync();

        var result = await new OrgQueryHandlers(context).Handle(
            new GetCountriesQuery(false),
            CancellationToken.None
        );

        await Assert.That(result.Value).HasSingleItem();
        await Assert.That(result.Value[0].CountryCode).IsEqualTo("EG");
    }

    [Test]
    public async Task GetCountryByCode_IsCaseInsensitive()
    {
        await using var context = TestDbContextFactory.CreateOrgContext();
        context.Countries.Add(new Country { CountryCode = "AE", CountryName = "United Arab Emirates" });
        await context.SaveChangesAsync();

        var result = await new OrgQueryHandlers(context).Handle(
            new GetCountryByCodeQuery("ae"),
            CancellationToken.None
        );

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.Value.CountryName).IsEqualTo("United Arab Emirates");
    }

    [Test]
    public async Task GetCountryByCode_WhenMissingReturnsNotFound()
    {
        await using var context = TestDbContextFactory.CreateOrgContext();

        var result = await new OrgQueryHandlers(context).Handle(
            new GetCountryByCodeQuery("AE"),
            CancellationToken.None
        );

        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Error.Code).IsEqualTo("Country.NotFound");
    }
}
