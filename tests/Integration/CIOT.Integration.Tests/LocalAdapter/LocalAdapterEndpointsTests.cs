using System.Net;
using CIOT.Integration.Tests.Infrastructure;
using TUnit.Core;

namespace CIOT.Integration.Tests.LocalAdapter;

[ClassDataSource<ApiAppFixture>(Shared = SharedType.PerTestSession)]
public sealed class LocalAdapterEndpointsTests(ApiAppFixture fixture)
{
    [Test]
    public async Task UnknownDeviceEffects_ReturnsEmptyPersistedCollection(
        CancellationToken cancellationToken
    )
    {
        var response = await fixture.CreateApiClient().GetAsync(
            $"/api/local-adapter/devices/{Guid.NewGuid()}/effects",
            cancellationToken
        );

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var effects = await ApiIntegrationAssertions.ReadJsonAsync(response, cancellationToken);
        await Assert.That(effects.ValueKind).IsEqualTo(System.Text.Json.JsonValueKind.Array);
        await Assert.That(effects.GetArrayLength()).IsEqualTo(0);
    }

    [Test]
    public async Task DeviceEffectsEndpoint_DoesNotAllowWrites(CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"/api/local-adapter/devices/{Guid.NewGuid()}/effects"
        );
        var response = await fixture.CreateApiClient().SendAsync(request, cancellationToken);

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.MethodNotAllowed);
    }
}
