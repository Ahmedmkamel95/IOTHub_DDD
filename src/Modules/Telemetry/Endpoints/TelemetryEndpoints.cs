using CIOT.Modules.Telemetry.Application.Commands;
using CIOT.Modules.Telemetry.Application.Dtos;
using CIOT.Modules.Telemetry.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CIOT.Modules.Telemetry.Endpoints;

public static class TelemetryEndpoints
{
    public static IEndpointRouteBuilder MapTelemetryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/telemetry")
            .WithTags("Telemetry & Observability");

        group.MapPost("/ingest", async (TelemetryIngestRequest request, ISender sender) =>
        {
            var result = await sender.Send(new IngestTelemetryCommand(request));
            return result.IsSuccess
                ? Results.Ok(new { Count = result.Value, Status = "Ingested" })
                : Results.BadRequest(result.Error);
        })
        .WithName("IngestTelemetry")
        .WithSummary("Ingest normalized telemetry metrics from IoT device or gateway");

        group.MapPost("/raw", async (string payload, string? deviceId, string? assetId, ISender sender) =>
        {
            var result = await sender.Send(new IngestRawMessageCommand(payload, deviceId, assetId));
            return result.IsSuccess ? Results.Accepted($"/api/telemetry/raw/{result.Value.Id}", result.Value) : Results.BadRequest(result.Error);
        })
        .WithName("IngestRawMessage")
        .WithSummary("Ingest raw telemetry payload directly into ingestion queue");

        group.MapGet("/assets/{assetId:guid}/state", async (Guid assetId, ISender sender) =>
        {
            var result = await sender.Send(new GetLatestAssetStateQuery(assetId));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        })
        .WithName("GetAssetCurrentState")
        .WithSummary("Get latest digital twin current state for an asset");

        group.MapGet("/measurements", async (Guid? deviceId, Guid? assetId, string? metricKey, int? limit, ISender sender) =>
        {
            var result = await sender.Send(new GetMeasurementsQuery(deviceId, assetId, metricKey, limit ?? 50));
            return Results.Ok(result.Value);
        })
        .WithName("GetMeasurements")
        .WithSummary("Query time-series measurements");

        return app;
    }
}
