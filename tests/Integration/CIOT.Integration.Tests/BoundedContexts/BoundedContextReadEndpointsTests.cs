using System.Net;
using CIOT.Integration.Tests.Infrastructure;
using TUnit.Core;

namespace CIOT.Integration.Tests.BoundedContexts;

[ClassDataSource<ApiAppFixture>(Shared = SharedType.PerTestSession)]
public sealed class BoundedContextReadEndpointsTests(ApiAppFixture fixture)
{
    [Test]
    public Task Admin_ReturnsEquipmentModels() =>
        AssertSuccessfulJsonAsync("/api/admin/equipment-models");

    [Test]
    public Task Asset_ReturnsAssets() =>
        AssertSuccessfulJsonAsync("/api/assets/");

    [Test]
    public Task Audit_ReturnsEvents() =>
        AssertSuccessfulJsonAsync("/api/audit/events");

    [Test]
    public Task Catalog_ReturnsMaterials() =>
        AssertSuccessfulJsonAsync("/api/catalog/materials");

    [Test]
    public Task CustomerOutlet_ReturnsCustomers() =>
        AssertSuccessfulJsonAsync("/api/customer-outlets/customers");

    [Test]
    public Task Devices_ReturnsDevices() =>
        AssertSuccessfulJsonAsync("/api/devices/");

    [Test]
    public Task Identity_ReturnsUsers() =>
        AssertSuccessfulJsonAsync("/api/identity/users");

    [Test]
    public Task Integration_ReturnsPartners() =>
        AssertSuccessfulJsonAsync("/api/integration/partners");

    [Test]
    public Task Provisioning_ReturnsManufacturers() =>
        AssertSuccessfulJsonAsync("/api/provisioning/manufacturers");

    [Test]
    public Task Report_ReturnsDefinitions() =>
        AssertSuccessfulJsonAsync("/api/reports/definitions");

    [Test]
    public Task Telemetry_ReturnsMeasurements() =>
        AssertSuccessfulJsonAsync("/api/telemetry/measurements");

    [Test]
    public Task Org_ReturnsCountries() =>
        AssertSuccessfulJsonAsync("/api/org/countries");

    [Test]
    public Task Mobile_ReturnsMethodNotAllowedForReadOnlyRoute() =>
        AssertStatusAsync("/api/mobile/sync-batch", HttpMethod.Get, HttpStatusCode.MethodNotAllowed);

    [Test]
    public Task LocalAdapter_ReturnsEmptyEffectsForUnknownDevice() =>
        AssertStatusAsync(
            $"/api/local-adapter/devices/{Guid.NewGuid():D}/effects",
            HttpMethod.Get,
            HttpStatusCode.OK
        );

    private async Task AssertSuccessfulJsonAsync(string route)
    {
        var response = await fixture.CreateApiClient().GetAsync(route);

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        await Assert.That(response.Content.Headers.ContentType?.MediaType)
            .IsEqualTo("application/json");
    }

    private async Task AssertStatusAsync(
        string route,
        HttpMethod method,
        HttpStatusCode expectedStatusCode
    )
    {
        using var request = new HttpRequestMessage(method, route);
        var response = await fixture.CreateApiClient().SendAsync(request);

        await Assert.That(response.StatusCode).IsEqualTo(expectedStatusCode);
    }
}
