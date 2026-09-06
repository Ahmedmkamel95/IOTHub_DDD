using System.Net;
using System.Net.Http.Json;
using CIOT.Integration.Tests.Infrastructure;
using TUnit.Core;

namespace CIOT.Integration.Tests.CustomerOutlet;

[ClassDataSource<ApiAppFixture>(Shared = SharedType.PerTestSession)]
public sealed class CustomerOutletEndpointsTests(ApiAppFixture fixture)
{
    [Test]
    public async Task CreateCustomer_ThenGetById_ReturnsPersistedCustomer(
        CancellationToken cancellationToken
    )
    {
        var customerCode = $"CUST-{Guid.NewGuid():N}";
        var response = await fixture.CreateApiClient().PostAsJsonAsync(
            "/api/customer-outlets/customers",
            new
            {
                customerCode,
                customerName1 = "Integration Customer",
                customerName2 = (string?)null,
                countryCode = "EG",
                vatNumber = (string?)null,
                wholesalerCode = (string?)null,
                customerClusterId = (Guid?)null
            },
            cancellationToken
        );

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.Created);
        var created = await ApiIntegrationAssertions.ReadJsonAsync(response, cancellationToken);
        var customerId = ApiIntegrationAssertions.GetId(created);

        var getResponse = await fixture.CreateApiClient().GetAsync(
            $"/api/customer-outlets/customers/{customerId}",
            cancellationToken
        );

        await Assert.That(getResponse.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var document = await ApiIntegrationAssertions.ReadJsonAsync(getResponse, cancellationToken);
        await Assert.That(document.GetProperty("customerCode").GetString()).IsEqualTo(customerCode.ToUpperInvariant());
        await Assert.That(document.GetProperty("countryCode").GetString()).IsEqualTo("EG");
        await Assert.That(document.GetProperty("isActive").GetBoolean()).IsTrue();
    }

    [Test]
    public async Task CreateClusterCustomerAndOutlet_ThenValidateClusterAndAddNote(
        CancellationToken cancellationToken
    )
    {
        var client = fixture.CreateApiClient();
        var suffix = Guid.NewGuid().ToString("N");

        var clusterResponse = await client.PostAsJsonAsync(
            "/api/customer-outlets/clusters",
            new
            {
                clusterCode = $"CL-{suffix}",
                clusterName = "Integration Cluster",
                description = "Created by integration test",
                isActive = true
            },
            cancellationToken
        );
        await Assert.That(clusterResponse.StatusCode).IsEqualTo(HttpStatusCode.Created);
        var cluster = await ApiIntegrationAssertions.ReadJsonAsync(clusterResponse, cancellationToken);
        var clusterId = ApiIntegrationAssertions.GetId(cluster);

        var customerResponse = await client.PostAsJsonAsync(
            "/api/customer-outlets/customers",
            new
            {
                customerCode = $"CUST-{suffix}",
                customerName1 = "Cluster Customer",
                customerName2 = (string?)null,
                countryCode = "EG",
                vatNumber = (string?)null,
                wholesalerCode = (string?)null,
                customerClusterId = clusterId
            },
            cancellationToken
        );
        await Assert.That(customerResponse.StatusCode).IsEqualTo(HttpStatusCode.Created);
        var customer = await ApiIntegrationAssertions.ReadJsonAsync(customerResponse, cancellationToken);
        var customerId = ApiIntegrationAssertions.GetId(customer);

        var validateResponse = await client.GetAsync(
            $"/api/customer-outlets/customers/{customerId}/validate-cluster?clusterId={clusterId}",
            cancellationToken
        );
        await Assert.That(validateResponse.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var validation = await ApiIntegrationAssertions.ReadJsonAsync(validateResponse, cancellationToken);
        await Assert.That(validation.GetProperty("isValid").GetBoolean()).IsTrue();
        await Assert.That(validation.GetProperty("clusterId").GetGuid()).IsEqualTo(clusterId);

        var outletResponse = await client.PostAsJsonAsync(
            "/api/customer-outlets/outlets",
            new
            {
                outletCode = $"OUT-{suffix}",
                customerId,
                outletType = "Retail",
                addressLine = "1 Integration Street",
                city = "Cairo",
                postalCode = "11511",
                countryCode = "EG",
                latitude = (decimal?)null,
                longitude = (decimal?)null,
                salesTerritoryId = (Guid?)null
            },
            cancellationToken
        );
        await Assert.That(outletResponse.StatusCode).IsEqualTo(HttpStatusCode.Created);
        var outlet = await ApiIntegrationAssertions.ReadJsonAsync(outletResponse, cancellationToken);
        var outletId = ApiIntegrationAssertions.GetId(outlet);

        var noteResponse = await client.PostAsJsonAsync(
            $"/api/customer-outlets/outlets/{outletId}/notes",
            new { noteBody = "Integration note", relatedAssetId = (Guid?)null },
            cancellationToken
        );
        await Assert.That(noteResponse.StatusCode).IsEqualTo(HttpStatusCode.Created);
        var note = await ApiIntegrationAssertions.ReadJsonAsync(noteResponse, cancellationToken);
        await Assert.That(note.GetProperty("outletId").GetGuid()).IsEqualTo(outletId);
        await Assert.That(note.GetProperty("noteBody").GetString()).IsEqualTo("Integration note");
    }

    [Test]
    public async Task DuplicateCustomer_ReturnsConflictError()
    {
        var client = fixture.CreateApiClient();
        var customerCode = $"DUP-{Guid.NewGuid():N}";
        var request = new
        {
            customerCode,
            customerName1 = "Duplicate Customer",
            customerName2 = (string?)null,
            countryCode = "EG",
            vatNumber = (string?)null,
            wholesalerCode = (string?)null,
            customerClusterId = (Guid?)null
        };

        var first = await client.PostAsJsonAsync("/api/customer-outlets/customers", request);
        await Assert.That(first.StatusCode).IsEqualTo(HttpStatusCode.Created);

        var duplicate = await client.PostAsJsonAsync("/api/customer-outlets/customers", request);
        await Assert.That(duplicate.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
        await ApiIntegrationAssertions.AssertErrorCodeAsync(
            duplicate,
            "Customer.Duplicate",
            CancellationToken.None
        );
    }

    [Test]
    public async Task UnknownCustomerAndOutlet_ReturnNotFound()
    {
        var client = fixture.CreateApiClient();
        var customerResponse = await client.GetAsync(
            $"/api/customer-outlets/customers/{Guid.NewGuid()}",
            CancellationToken.None
        );
        await Assert.That(customerResponse.StatusCode).IsEqualTo(HttpStatusCode.NotFound);

        var outletResponse = await client.GetAsync(
            $"/api/customer-outlets/outlets/{Guid.NewGuid()}",
            CancellationToken.None
        );
        await Assert.That(outletResponse.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
    }
}
