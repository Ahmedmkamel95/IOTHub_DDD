using System.Net;
using System.Net.Http.Json;
using CIOT.Integration.Tests.Infrastructure;
using TUnit.Core;

namespace CIOT.Integration.Tests.Admin;

[ClassDataSource<ApiAppFixture>(Shared = SharedType.PerTestSession)]
public sealed class AdminEndpointsTests(ApiAppFixture fixture)
{
    [Test]
    public async Task CreateEquipmentModel_ThenListModels_ReturnsPersistedModel(
        CancellationToken cancellationToken
    )
    {
        var client = fixture.CreateApiClient();
        var model = new
        {
            manufacturer = $"ADMIN-MFR-{Guid.NewGuid():N}",
            model = "Integration Model",
            machineType = "CoffeeMachine"
        };

        var createResponse = await client.PostAsJsonAsync(
            "/api/admin/equipment-models",
            model,
            cancellationToken
        );
        await Assert.That(createResponse.StatusCode).IsEqualTo(HttpStatusCode.Created);
        var created = await ApiIntegrationAssertions.ReadJsonAsync(createResponse, cancellationToken);
        await Assert.That(created.GetProperty("manufacturer").GetString())
            .IsEqualTo(model.manufacturer);
        await Assert.That(created.GetProperty("isActive").GetBoolean()).IsTrue();

        var listResponse = await client.GetAsync("/api/admin/equipment-models", cancellationToken);
        await Assert.That(listResponse.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var models = await ApiIntegrationAssertions.ReadJsonAsync(listResponse, cancellationToken);
        await Assert.That(
            models.EnumerateArray().Any(item =>
                item.GetProperty("id").GetGuid() == created.GetProperty("id").GetGuid())
        ).IsTrue();
    }
}
