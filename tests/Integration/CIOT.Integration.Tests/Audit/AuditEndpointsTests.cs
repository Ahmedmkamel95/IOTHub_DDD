using System.Net;
using CIOT.Integration.Tests.Infrastructure;
using TUnit.Core;

namespace CIOT.Integration.Tests.Audit;

[ClassDataSource<ApiAppFixture>(Shared = SharedType.PerTestSession)]
public sealed class AuditEndpointsTests(ApiAppFixture fixture)
{
    [Test]
    public async Task EventsEndpoint_WithGeneratedEntityFilter_ReturnsEmptyPersistedCollection(
        CancellationToken cancellationToken
    )
    {
        var response = await fixture.CreateApiClient().GetAsync(
            $"/api/audit/events?entityType=IntegrationTest&entityId={Guid.NewGuid():N}&limit=10",
            cancellationToken
        );

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var events = await ApiIntegrationAssertions.ReadJsonAsync(response, cancellationToken);
        await Assert.That(events.ValueKind).IsEqualTo(System.Text.Json.JsonValueKind.Array);
        await Assert.That(events.GetArrayLength()).IsEqualTo(0);
    }

    [Test]
    public async Task EventsEndpoint_DoesNotAllowWrites(CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/audit/events");
        var response = await fixture.CreateApiClient().SendAsync(request, cancellationToken);

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.MethodNotAllowed);
    }
}
