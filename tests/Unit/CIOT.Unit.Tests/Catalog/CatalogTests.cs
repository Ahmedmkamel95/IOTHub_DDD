using CIOT.Modules.Catalog.Application;
using CIOT.Modules.Catalog.Application.Dtos;
using CIOT.Modules.Catalog.Domain;
using CIOT.Unit.Tests.TestInfrastructure;
using Microsoft.EntityFrameworkCore;
using TUnit.Assertions;
using TUnit.Core;

namespace CIOT.Unit.Tests.Catalog;

public sealed class CatalogTests
{
    [Test]
    public async Task CreateMaterial_NormalizesCountryAndPersistsMaterial()
    {
        await using var context = TestDbContextFactory.CreateCatalogContext();
        var result = await new CatalogHandlers(context).Handle(
            new CreateMaterialCommand(new CreateMaterialRequest("MAT-001", "Coffee", "ae")),
            CancellationToken.None
        );

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.Value.CountryCode).IsEqualTo("AE");
        await Assert.That(await context.Materials.CountAsync()).IsEqualTo(1);
    }

    [Test]
    public async Task CreateMaterial_WhenSameCodeAndCountryExists_ReturnsConflict()
    {
        await using var context = TestDbContextFactory.CreateCatalogContext();
        context.Materials.Add(new Material { MaterialCode = "MAT-001", ProductName = "Coffee", CountryCode = "AE" });
        await context.SaveChangesAsync();

        var result = await new CatalogHandlers(context).Handle(
            new CreateMaterialCommand(new CreateMaterialRequest("MAT-001", "Duplicate", "AE")),
            CancellationToken.None
        );

        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Error.Code).IsEqualTo("Material.Duplicate");
    }

    [Test]
    public async Task GetMaterials_FiltersCountryCaseInsensitively()
    {
        await using var context = TestDbContextFactory.CreateCatalogContext();
        context.Materials.AddRange(
            new Material { MaterialCode = "MAT-002", ProductName = "Water", CountryCode = "AE" },
            new Material { MaterialCode = "MAT-001", ProductName = "Coffee", CountryCode = "EG" }
        );
        await context.SaveChangesAsync();

        var result = await new CatalogHandlers(context).Handle(new GetMaterialsQuery("ae"), CancellationToken.None);

        await Assert.That(result.Value).HasSingleItem();
        await Assert.That(result.Value[0].MaterialCode).IsEqualTo("MAT-002");
    }
}
