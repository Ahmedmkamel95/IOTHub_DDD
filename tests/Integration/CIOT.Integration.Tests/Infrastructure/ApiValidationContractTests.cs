using System.Net;
using System.Net.Http.Json;
using TUnit.Core;

namespace CIOT.Integration.Tests.Infrastructure;

[ClassDataSource<ApiAppFixture>(Shared = SharedType.PerTestSession)]
public sealed class ApiValidationContractTests(ApiAppFixture fixture)
{
    [Test]
    public async Task CreateCountry_WithMissingRequiredFields_ReturnsBadRequest(
        CancellationToken cancellationToken
    )
    {
        var response = await fixture.CreateApiClient().PostAsJsonAsync(
            "/api/org/countries",
            new { countryCode = string.Empty, countryName = string.Empty, defaultTimezone = "UTC" },
            cancellationToken
        );

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task RegisterDevice_WithMissingHubIdentifier_ReturnsBadRequest(
        CancellationToken cancellationToken
    )
    {
        var response = await fixture.CreateApiClient().PostAsJsonAsync(
            "/api/devices/",
            new
            {
                iotHubDeviceId = string.Empty,
                deviceSerialNumber = "SERIAL-TEST",
                imei = (string?)null,
                macAddress = (string?)null,
                countryCode = "EG",
                firmwareVersion = "1.0.0"
            },
            cancellationToken
        );

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task CreateCountry_WithMalformedJson_ReturnsBadRequest(
        CancellationToken cancellationToken
    )
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/org/countries")
        {
            Content = new StringContent(
                """{"countryCode":"EG","countryName":""",
                System.Text.Encoding.UTF8,
                "application/json"
            )
        };

        var response = await fixture.CreateApiClient().SendAsync(request, cancellationToken);

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task CountriesEndpoint_WithUnsupportedMethod_ReturnsMethodNotAllowed(
        CancellationToken cancellationToken
    )
    {
        using var request = new HttpRequestMessage(HttpMethod.Delete, "/api/org/countries");

        var response = await fixture.CreateApiClient().SendAsync(request, cancellationToken);

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.MethodNotAllowed);
    }
}
