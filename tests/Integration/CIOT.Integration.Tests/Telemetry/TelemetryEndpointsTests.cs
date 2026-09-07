using System.Net;
using System.Net.Http.Json;
using CIOT.Integration.Tests.Infrastructure;
using TUnit.Core;

namespace CIOT.Integration.Tests.Telemetry;

[ClassDataSource<ApiAppFixture>(Shared = SharedType.PerTestSession)]
public sealed class TelemetryEndpointsTests(ApiAppFixture fixture)
{
    [Test]
    public async Task IngestTelemetry_ThenReadMeasurementsAndCurrentState(
        CancellationToken cancellationToken
    )
    {
        var client = fixture.CreateApiClient();
        var assetId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();
        var timestamp = DateTime.UtcNow.AddMinutes(-1);

        var ingestResponse = await client.PostAsJsonAsync(
            "/api/telemetry/ingest",
            new
            {
                deviceId,
                assetId,
                timestampUtc = timestamp,
                metrics = new[]
                {
                    new { metricKey = "cups", value = 42d, unit = "count" },
                    new { metricKey = "energy_kwh", value = 1.5d, unit = "kWh" }
                },
                machineStatus = "Running",
                latitude = 30.0444m,
                longitude = 31.2357m
            },
            cancellationToken
        );

        await Assert.That(ingestResponse.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var ingest = await ApiIntegrationAssertions.ReadJsonAsync(ingestResponse, cancellationToken);
        await Assert.That(ingest.GetProperty("count").GetInt32()).IsEqualTo(2);
        await Assert.That(ingest.GetProperty("status").GetString()).IsEqualTo("Ingested");

        var measurementsResponse = await client.GetAsync(
            $"/api/telemetry/measurements?assetId={assetId}&metricKey=cups",
            cancellationToken
        );
        await Assert.That(measurementsResponse.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var measurements = await ApiIntegrationAssertions.ReadJsonAsync(
            measurementsResponse,
            cancellationToken
        );
        await Assert.That(measurements.GetArrayLength()).IsEqualTo(1);
        await Assert.That(measurements[0].GetProperty("metricKey").GetString()).IsEqualTo("cups");
        await Assert.That(measurements[0].GetProperty("numericValue").GetDouble()).IsEqualTo(42d);

        var stateResponse = await client.GetAsync(
            $"/api/telemetry/assets/{assetId}/state",
            cancellationToken
        );
        await Assert.That(stateResponse.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var state = await ApiIntegrationAssertions.ReadJsonAsync(stateResponse, cancellationToken);
        await Assert.That(state.GetProperty("assetId").GetGuid()).IsEqualTo(assetId);
        await Assert.That(state.GetProperty("deviceId").GetGuid()).IsEqualTo(deviceId);
        await Assert.That(state.GetProperty("machineStatus").GetString()).IsEqualTo("Running");
        await Assert.That(state.GetProperty("cupsToday").GetInt32()).IsEqualTo(42);
    }

    [Test]
    public async Task IngestRawMessage_ReturnsAcceptedMessage(CancellationToken cancellationToken)
    {
        var payload = """{"event":"integration-test"}""";
        var response = await fixture.CreateApiClient().PostAsync(
            $"/api/telemetry/raw?payload={Uri.EscapeDataString(payload)}&deviceId=device-{Guid.NewGuid():N}",
            content: null,
            cancellationToken
        );

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.Accepted);
        var document = await ApiIntegrationAssertions.ReadJsonAsync(response, cancellationToken);
        await Assert.That(document.GetProperty("status").GetString()).IsEqualTo("Received");
        await Assert.That(document.GetProperty("messageId").GetString()).IsNotNull();
    }

    [Test]
    public async Task TelemetryValidationAndMissingState_ReturnDomainErrors(
        CancellationToken cancellationToken
    )
    {
        var client = fixture.CreateApiClient();
        var invalidResponse = await client.PostAsJsonAsync(
            "/api/telemetry/ingest",
            new
            {
                deviceId = (Guid?)null,
                assetId = (Guid?)null,
                timestampUtc = (DateTime?)null,
                metrics = Array.Empty<object>(),
                machineStatus = (string?)null,
                latitude = (decimal?)null,
                longitude = (decimal?)null
            },
            cancellationToken
        );
        await Assert.That(invalidResponse.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

        var missingStateResponse = await client.GetAsync(
            $"/api/telemetry/assets/{Guid.NewGuid()}/state",
            cancellationToken
        );
        await Assert.That(missingStateResponse.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
    }
}
