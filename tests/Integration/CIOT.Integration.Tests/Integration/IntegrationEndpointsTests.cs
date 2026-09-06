using System.Net;
using System.Net.Http.Json;
using CIOT.Integration.Tests.Infrastructure;
using TUnit.Core;

namespace CIOT.Integration.Tests.Integration;

[ClassDataSource<ApiAppFixture>(Shared = SharedType.PerTestSession)]
public sealed class IntegrationEndpointsTests(ApiAppFixture fixture)
{
    [Test]
    public async Task CreatePartnerSource_ThenListPartners_ReturnsNormalizedSource(
        CancellationToken cancellationToken
    )
    {
        var client = fixture.CreateApiClient();
        var sourceCode = $"partner-{Guid.NewGuid():N}";
        var createResponse = await client.PostAsJsonAsync(
            "/api/integration/partners",
            new
            {
                sourceCode,
                displayName = "Integration Partner",
                integrationType = "REST",
                endpointUrl = "https://partner.example.test/api"
            },
            cancellationToken
        );

        await Assert.That(createResponse.StatusCode).IsEqualTo(HttpStatusCode.Created);
        var created = await ApiIntegrationAssertions.ReadJsonAsync(createResponse, cancellationToken);
        await Assert.That(created.GetProperty("sourceCode").GetString())
            .IsEqualTo(sourceCode.ToUpperInvariant());
        await Assert.That(created.GetProperty("integrationType").GetString()).IsEqualTo("REST");
        await Assert.That(created.GetProperty("isActive").GetBoolean()).IsTrue();

        var listResponse = await client.GetAsync("/api/integration/partners", cancellationToken);
        await Assert.That(listResponse.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var partners = await ApiIntegrationAssertions.ReadJsonAsync(listResponse, cancellationToken);
        await Assert.That(
            partners.EnumerateArray().Any(item =>
                item.GetProperty("id").GetGuid() == created.GetProperty("id").GetGuid())
        ).IsTrue();
    }
}
