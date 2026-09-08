using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CIOT.Integration.Tests.Infrastructure;
using TUnit.Core;

namespace CIOT.Integration.Tests.Devices;

[ClassDataSource<ApiAppFixture>(Shared = SharedType.PerTestSession)]
public sealed class DevicesEndpointsTests(ApiAppFixture fixture)
{
    [Test]
    public async Task RegisterDevice_ThenGetById_PersistsThroughApi(
        CancellationToken cancellationToken
    )
    {
        var client = fixture.CreateApiClient();
        var deviceId = $"integration-device-{Guid.NewGuid():N}";

        var createResponse = await client.PostAsJsonAsync(
            "/api/devices/",
            new
            {
                iotHubDeviceId = deviceId,
                deviceSerialNumber = "SERIAL-INTEGRATION",
                imei = (string?)null,
                macAddress = "00:11:22:33:44:55",
                countryCode = "EG",
                firmwareVersion = "1.0.0"
            },
            cancellationToken
        );

        await Assert.That(createResponse.StatusCode).IsEqualTo(HttpStatusCode.Created);
        var created = await createResponse.Content.ReadFromJsonAsync<JsonElement>(cancellationToken);
        var id = created.GetProperty("id").GetGuid();

        var getResponse = await client.GetAsync($"/api/devices/{id}", cancellationToken);

        await Assert.That(getResponse.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var document = await getResponse.Content.ReadFromJsonAsync<JsonElement>(cancellationToken);
        await Assert.That(document.GetProperty("iotHubDeviceId").GetString()).IsEqualTo(deviceId);
        await Assert.That(document.GetProperty("lifecycleStatus").GetString()).IsEqualTo("Registered");
    }
}
