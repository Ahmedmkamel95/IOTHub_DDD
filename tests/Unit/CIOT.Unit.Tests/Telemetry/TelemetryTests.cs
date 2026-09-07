using CIOT.Modules.Telemetry.Application.Commands;
using CIOT.Modules.Telemetry.Application.Dtos;
using CIOT.Unit.Tests.TestInfrastructure;
using Microsoft.EntityFrameworkCore;
using TUnit.Assertions;
using TUnit.Core;

namespace CIOT.Unit.Tests.Telemetry;

public sealed class TelemetryTests
{
    [Test]
    public async Task IngestTelemetry_PersistsMeasurementsAndUpdatesCurrentState()
    {
        await using var context = TestDbContextFactory.CreateTelemetryContext();
        var assetId = Guid.NewGuid();
        var timestamp = new DateTime(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        var result = await new TelemetryCommandHandlers(context).Handle(
            new IngestTelemetryCommand(new TelemetryIngestRequest(
                Guid.NewGuid(), assetId, timestamp,
                [new MetricValueDto("cups", 12, "count"), new MetricValueDto("water_liters", 3.5, "L")],
                "Operational", 25.2m, 55.3m)),
            CancellationToken.None
        );

        var state = await context.AssetCurrentStates.SingleAsync();
        await Assert.That(result.Value).IsEqualTo(2);
        await Assert.That(await context.NormalizedMeasurements.CountAsync()).IsEqualTo(2);
        await Assert.That(state.CupsToday).IsEqualTo(12);
        await Assert.That(state.WaterLitersToday).IsEqualTo(3.5m);
        await Assert.That(state.MachineStatus).IsEqualTo("Operational");
    }

    [Test]
    public async Task IngestTelemetryValidator_RequiresMetrics()
    {
        var result = await new IngestTelemetryCommandValidator().ValidateAsync(
            new IngestTelemetryCommand(new TelemetryIngestRequest(null, null, null, []))
        );

        await Assert.That(result.IsValid).IsFalse();
    }

    [Test]
    public async Task IngestRawMessage_SetsReceivedStatus()
    {
        await using var context = TestDbContextFactory.CreateTelemetryContext();
        var result = await new TelemetryCommandHandlers(context).Handle(
            new IngestRawMessageCommand("{\"temperature\":20}", "device-1", null),
            CancellationToken.None
        );

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.Value.Status).IsEqualTo("Received");
        await Assert.That(await context.RawMessages.CountAsync()).IsEqualTo(1);
    }
}
