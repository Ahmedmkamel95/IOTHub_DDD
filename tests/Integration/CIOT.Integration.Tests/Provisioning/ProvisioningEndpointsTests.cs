using System.Net;
using System.Net.Http.Json;
using CIOT.Integration.Tests.Infrastructure;
using TUnit.Core;

namespace CIOT.Integration.Tests.Provisioning;

[ClassDataSource<ApiAppFixture>(Shared = SharedType.PerTestSession)]
public sealed class ProvisioningEndpointsTests(ApiAppFixture fixture)
{
    [Test]
    public async Task CreateManufacturerAndModel_ThenFilterModels_ReturnsPersistedHardware(
        CancellationToken cancellationToken
    )
    {
        var client = fixture.CreateApiClient();
        var suffix = Guid.NewGuid().ToString("N");
        var manufacturerResponse = await client.PostAsJsonAsync(
            "/api/provisioning/manufacturers",
            new
            {
                manufacturerCode = $"MFR-{suffix}",
                displayName = "Integration Manufacturer"
            },
            cancellationToken
        );
        await Assert.That(manufacturerResponse.StatusCode).IsEqualTo(HttpStatusCode.Created);
        var manufacturer = await ApiIntegrationAssertions.ReadJsonAsync(
            manufacturerResponse,
            cancellationToken
        );
        var manufacturerId = ApiIntegrationAssertions.GetId(manufacturer);

        var modelResponse = await client.PostAsJsonAsync(
            "/api/provisioning/models",
            new
            {
                deviceManufacturerId = manufacturerId,
                modelCode = $"MODEL-{suffix}",
                displayName = "Integration Model",
                hardwareRevision = "R1"
            },
            cancellationToken
        );
        await Assert.That(modelResponse.StatusCode).IsEqualTo(HttpStatusCode.Created);
        var model = await ApiIntegrationAssertions.ReadJsonAsync(modelResponse, cancellationToken);
        await Assert.That(model.GetProperty("deviceManufacturerId").GetGuid()).IsEqualTo(manufacturerId);
        await Assert.That(model.GetProperty("status").GetString()).IsEqualTo("Active");

        var listResponse = await client.GetAsync(
            $"/api/provisioning/models?manufacturerId={manufacturerId}",
            cancellationToken
        );
        await Assert.That(listResponse.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var models = await ApiIntegrationAssertions.ReadJsonAsync(listResponse, cancellationToken);
        await Assert.That(
            models.EnumerateArray().Any(item =>
                item.GetProperty("id").GetGuid() == model.GetProperty("id").GetGuid())
        ).IsTrue();
    }

    [Test]
    public async Task DuplicateManufacturer_ReturnsConflictError(CancellationToken cancellationToken)
    {
        var client = fixture.CreateApiClient();
        var request = new
        {
            manufacturerCode = $"DUP-MFR-{Guid.NewGuid():N}",
            displayName = "Duplicate Manufacturer"
        };

        var first = await client.PostAsJsonAsync(
            "/api/provisioning/manufacturers",
            request,
            cancellationToken
        );
        await Assert.That(first.StatusCode).IsEqualTo(HttpStatusCode.Created);

        var duplicate = await client.PostAsJsonAsync(
            "/api/provisioning/manufacturers",
            request,
            cancellationToken
        );
        await Assert.That(duplicate.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
        await ApiIntegrationAssertions.AssertErrorCodeAsync(
            duplicate,
            "Manufacturer.Duplicate",
            cancellationToken
        );
    }

    [Test]
    public async Task CreateModelForUnknownManufacturer_ReturnsNotFoundDomainError(
        CancellationToken cancellationToken
    )
    {
        var response = await fixture.CreateApiClient().PostAsJsonAsync(
            "/api/provisioning/models",
            new
            {
                deviceManufacturerId = Guid.NewGuid(),
                modelCode = $"MODEL-{Guid.NewGuid():N}",
                displayName = "Unknown Manufacturer Model",
                hardwareRevision = (string?)null
            },
            cancellationToken
        );

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
        await ApiIntegrationAssertions.AssertErrorCodeAsync(
            response,
            "Manufacturer.NotFound",
            cancellationToken
        );
    }
}
