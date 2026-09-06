using System.Net;
using System.Net.Http.Json;
using CIOT.Integration.Tests.Infrastructure;
using TUnit.Core;

namespace CIOT.Integration.Tests.Catalog;

[ClassDataSource<ApiAppFixture>(Shared = SharedType.PerTestSession)]
public sealed class CatalogEndpointsTests(ApiAppFixture fixture)
{
    [Test]
    public async Task CreateMaterial_ThenFilterByCountry_ReturnsPersistedMaterial(
        CancellationToken cancellationToken
    )
    {
        var client = fixture.CreateApiClient();
        var materialCode = $"MAT-{Guid.NewGuid():N}";
        var response = await client.PostAsJsonAsync(
            "/api/catalog/materials",
            new
            {
                materialCode,
                productName = "Integration Material",
                countryCode = "eg",
                businessUnitId = (Guid?)null
            },
            cancellationToken
        );

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.Created);
        var created = await ApiIntegrationAssertions.ReadJsonAsync(response, cancellationToken);
        await Assert.That(created.GetProperty("materialCode").GetString()).IsEqualTo(materialCode);
        await Assert.That(created.GetProperty("countryCode").GetString()).IsEqualTo("EG");

        var listResponse = await client.GetAsync("/api/catalog/materials?countryCode=EG", cancellationToken);
        await Assert.That(listResponse.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var materials = await ApiIntegrationAssertions.ReadJsonAsync(listResponse, cancellationToken);
        await Assert.That(
            materials.EnumerateArray().Any(item =>
                item.GetProperty("materialCode").GetString() == materialCode)
        ).IsTrue();
    }

    [Test]
    public async Task DuplicateMaterialInSameCountry_ReturnsConflictError(
        CancellationToken cancellationToken
    )
    {
        var client = fixture.CreateApiClient();
        var request = new
        {
            materialCode = $"DUP-MAT-{Guid.NewGuid():N}",
            productName = "Duplicate Material",
            countryCode = "EG",
            businessUnitId = (Guid?)null
        };

        var first = await client.PostAsJsonAsync("/api/catalog/materials", request, cancellationToken);
        await Assert.That(first.StatusCode).IsEqualTo(HttpStatusCode.Created);

        var duplicate = await client.PostAsJsonAsync(
            "/api/catalog/materials",
            request,
            cancellationToken
        );
        await Assert.That(duplicate.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
        await ApiIntegrationAssertions.AssertErrorCodeAsync(
            duplicate,
            "Material.Duplicate",
            cancellationToken
        );
    }
}
