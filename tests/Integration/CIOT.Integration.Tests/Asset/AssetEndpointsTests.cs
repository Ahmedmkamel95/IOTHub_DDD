using System.Net;
using System.Net.Http.Json;
using CIOT.Integration.Tests.Infrastructure;
using TUnit.Core;

namespace CIOT.Integration.Tests.Asset;

[ClassDataSource<ApiAppFixture>(Shared = SharedType.PerTestSession)]
public sealed class AssetEndpointsTests(ApiAppFixture fixture)
{
    [Test]
    public async Task RegisterAsset_ThenGetByIdAndAssignments_ReturnsPersistedData(
        CancellationToken cancellationToken
    )
    {
        var client = fixture.CreateApiClient();
        var equipmentNumber = $"EQ-{Guid.NewGuid():N}";
        var createResponse = await client.PostAsJsonAsync(
            "/api/assets/",
            new
            {
                sapEquipmentNumber = equipmentNumber,
                oemSerialNumber = "OEM-INTEGRATION",
                technicalId = "TECH-INTEGRATION",
                equipmentModelId = (Guid?)null,
                countryCode = "eg",
                sapStatus = "Installed"
            },
            cancellationToken
        );

        await Assert.That(createResponse.StatusCode).IsEqualTo(HttpStatusCode.Created);
        var created = await ApiIntegrationAssertions.ReadJsonAsync(createResponse, cancellationToken);
        var assetId = ApiIntegrationAssertions.GetId(created);

        var assignResponse = await client.PostAsJsonAsync(
            $"/api/assets/{assetId}/assign-outlet",
            new { outletId = Guid.NewGuid(), customerId = (Guid?)null },
            cancellationToken
        );
        await Assert.That(assignResponse.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var assignment = await ApiIntegrationAssertions.ReadJsonAsync(assignResponse, cancellationToken);
        await Assert.That(assignment.GetProperty("assetId").GetGuid()).IsEqualTo(assetId);
        await Assert.That(assignment.GetProperty("isCurrent").GetBoolean()).IsTrue();

        var getResponse = await client.GetAsync($"/api/assets/{assetId}", cancellationToken);
        await Assert.That(getResponse.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var document = await ApiIntegrationAssertions.ReadJsonAsync(getResponse, cancellationToken);
        await Assert.That(document.GetProperty("sapEquipmentNumber").GetString())
            .IsEqualTo(equipmentNumber);
        await Assert.That(document.GetProperty("countryCode").GetString()).IsEqualTo("EG");
        await Assert.That(document.GetProperty("currentOutletId").GetGuid())
            .IsEqualTo(assignment.GetProperty("outletId").GetGuid());

        var assignmentsResponse = await client.GetAsync(
            $"/api/assets/{assetId}/assignments",
            cancellationToken
        );
        await Assert.That(assignmentsResponse.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var assignments = await ApiIntegrationAssertions.ReadJsonAsync(assignmentsResponse, cancellationToken);
        await Assert.That(assignments.GetArrayLength()).IsEqualTo(1);
    }

    [Test]
    public async Task DuplicateAsset_ReturnsConflictError(CancellationToken cancellationToken)
    {
        var client = fixture.CreateApiClient();
        var equipmentNumber = $"DUP-EQ-{Guid.NewGuid():N}";
        var request = new
        {
            sapEquipmentNumber = equipmentNumber,
            oemSerialNumber = (string?)null,
            technicalId = (string?)null,
            equipmentModelId = (Guid?)null,
            countryCode = "EG",
            sapStatus = (string?)null
        };

        var first = await client.PostAsJsonAsync("/api/assets/", request, cancellationToken);
        await Assert.That(first.StatusCode).IsEqualTo(HttpStatusCode.Created);

        var duplicate = await client.PostAsJsonAsync("/api/assets/", request, cancellationToken);
        await Assert.That(duplicate.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
        await ApiIntegrationAssertions.AssertErrorCodeAsync(
            duplicate,
            "Asset.Duplicate",
            cancellationToken
        );
    }

    [Test]
    public async Task UnknownAsset_ReadAndAssignment_ReturnDomainErrors(
        CancellationToken cancellationToken
    )
    {
        var client = fixture.CreateApiClient();
        var assetId = Guid.NewGuid();

        var getResponse = await client.GetAsync($"/api/assets/{assetId}", cancellationToken);
        await Assert.That(getResponse.StatusCode).IsEqualTo(HttpStatusCode.NotFound);

        var assignResponse = await client.PostAsJsonAsync(
            $"/api/assets/{assetId}/assign-outlet",
            new { outletId = Guid.NewGuid(), customerId = (Guid?)null },
            cancellationToken
        );
        await Assert.That(assignResponse.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
        await ApiIntegrationAssertions.AssertErrorCodeAsync(
            assignResponse,
            "Asset.NotFound",
            cancellationToken
        );
    }
}
