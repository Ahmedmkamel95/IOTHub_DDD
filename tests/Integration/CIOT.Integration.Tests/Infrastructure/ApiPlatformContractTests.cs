using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using TUnit.Core;

namespace CIOT.Integration.Tests.Infrastructure;

[ClassDataSource<ApiAppFixture>(Shared = SharedType.PerTestSession)]
public sealed class ApiPlatformContractTests(ApiAppFixture fixture)
{
    [Test]
    public async Task RootEndpoint_ReportsFourteenBoundedContexts(CancellationToken cancellationToken)
    {
        var response = await fixture.CreateApiClient().GetAsync("/", cancellationToken);

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        await Assert.That(response.Content.Headers.ContentType?.MediaType).IsEqualTo("application/json");
        var document = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken);
        await Assert.That(document.GetProperty("modulesCount").GetInt32()).IsEqualTo(14);
        await Assert.That(document.GetProperty("application").GetString()).IsEqualTo("CIOT_ModularHub");
        await Assert.That(document.GetProperty("status").GetString()).IsEqualTo("Healthy");
    }

    [Test]
    public async Task HealthEndpoint_ReturnsHealthy(CancellationToken cancellationToken)
    {
        var response = await fixture.CreateApiClient().GetAsync("/health", cancellationToken);

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        await Assert.That(response.Content.Headers.ContentType?.MediaType).IsEqualTo("text/plain");
    }

    [Test]
    public async Task UnknownRoute_ReturnsNotFound(CancellationToken cancellationToken)
    {
        var response = await fixture.CreateApiClient().GetAsync("/api/not-a-real-route", cancellationToken);

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task InvalidGuidRouteConstraint_ReturnsNotFound(CancellationToken cancellationToken)
    {
        var response = await fixture.CreateApiClient().GetAsync(
            "/api/devices/not-a-guid",
            cancellationToken
        );

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task AliveEndpoint_ReturnsHealthy(CancellationToken cancellationToken)
    {
        var response = await fixture.CreateApiClient().GetAsync("/alive", cancellationToken);

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        await Assert.That(response.Content.Headers.ContentType?.MediaType).IsEqualTo("text/plain");
    }

    [Test]
    public async Task OpenApiDocument_IsAvailable(CancellationToken cancellationToken)
    {
        var response = await fixture.CreateApiClient().GetAsync("/openapi/v1.json", cancellationToken);

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        await Assert.That(response.Content.Headers.ContentType?.MediaType).IsEqualTo("application/json");

        var document = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken);
        await Assert.That(document.TryGetProperty("paths", out var paths)).IsTrue();
        await Assert.That(paths.EnumerateObject().Any()).IsTrue();
    }

    [Test]
    public async Task OpenApiDocument_ExposesEveryBoundedContext(CancellationToken cancellationToken)
    {
        var response = await fixture.CreateApiClient().GetAsync("/openapi/v1.json", cancellationToken);
        var document = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken);
        var paths = document.GetProperty("paths")
            .EnumerateObject()
            .Select(static path => path.Name)
            .ToArray();

        var expectedPrefixes = new[]
        {
            "/api/admin/",
            "/api/assets/",
            "/api/audit/",
            "/api/catalog/",
            "/api/customer-outlets/",
            "/api/devices/",
            "/api/identity/",
            "/api/integration/",
            "/api/local-adapter/",
            "/api/mobile/",
            "/api/org/",
            "/api/provisioning/",
            "/api/reports/",
            "/api/telemetry/"
        };

        foreach (var prefix in expectedPrefixes)
        {
            await Assert.That(paths.Any(path => path.StartsWith(prefix, StringComparison.Ordinal)))
                .IsTrue();
        }
    }

    [Test]
    public async Task OpenApiDocument_HasUniqueOperationIds(CancellationToken cancellationToken)
    {
        var response = await fixture.CreateApiClient().GetAsync("/openapi/v1.json", cancellationToken);
        var document = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken);
        var operationIds = document
            .GetProperty("paths")
            .EnumerateObject()
            .SelectMany(static path => path.Value.EnumerateObject())
            .Where(static operation => operation.Value.TryGetProperty("operationId", out _))
            .Select(static operation => operation.Value.GetProperty("operationId").GetString())
            .Where(static operationId => operationId is not null)
            .ToArray();

        await Assert.That(operationIds).IsNotEmpty();
        await Assert.That(operationIds.Distinct(StringComparer.Ordinal).Count())
            .IsEqualTo(operationIds.Length);
    }

    [Test]
    public async Task OpenApiDocument_UsesSupportedHttpMethodsOnly(CancellationToken cancellationToken)
    {
        var response = await fixture.CreateApiClient().GetAsync("/openapi/v1.json", cancellationToken);
        var document = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken);
        var supportedMethods = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "get", "post", "put", "patch", "delete", "head", "options", "trace"
        };

        foreach (var path in document.GetProperty("paths").EnumerateObject())
        {
            foreach (var operation in path.Value.EnumerateObject())
            {
                await Assert.That(supportedMethods.Contains(operation.Name)).IsTrue();
            }
        }
    }
}
