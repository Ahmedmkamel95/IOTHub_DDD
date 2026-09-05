using CIOT.Modules.Devices.Application.Commands;
using CIOT.Modules.Devices.Application.Dtos;
using CIOT.Modules.Devices.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CIOT.Modules.Devices.Endpoints;

public static class DevicesEndpoints
{
    public static IEndpointRouteBuilder MapDevicesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/devices")
            .WithTags("Devices & Control");

        group.MapGet("/", async (string? status, int? page, int? pageSize, ISender sender) =>
        {
            var result = await sender.Send(new GetDevicesQuery(status, page ?? 1, pageSize ?? 20));
            return Results.Ok(result.Value);
        })
        .WithName("GetDevices")
        .WithSummary("List IoT devices");

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetDeviceByIdQuery(id));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        })
        .WithName("GetDeviceById")
        .WithSummary("Get device details");

        group.MapPost("/", async (RegisterDeviceRequest request, ISender sender) =>
        {
            var result = await sender.Send(new RegisterDeviceCommand(request));
            return result.IsSuccess
                ? Results.Created($"/api/devices/{result.Value.Id}", result.Value)
                : Results.BadRequest(result.Error);
        })
        .WithName("RegisterDevice")
        .WithSummary("Register a new IoT device in the Hub");

        group.MapPut("/{id:guid}/status", async (Guid id, UpdateDeviceStatusRequest request, ISender sender) =>
        {
            var result = await sender.Send(new UpdateDeviceStatusCommand(id, request.LifecycleStatus));
            return result.IsSuccess ? Results.NoContent() : Results.BadRequest(result.Error);
        })
        .WithName("UpdateDeviceStatus")
        .WithSummary("Change device lifecycle status (Active, Suspended, Decommissioned)");

        group.MapPost("/{id:guid}/pair", async (Guid id, PairDeviceRequest request, ISender sender) =>
        {
            var result = await sender.Send(new PairDeviceWithAssetCommand(id, request));
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
        })
        .WithName("PairDeviceWithAsset")
        .WithSummary("Pair an IoT device with a physical asset");

        group.MapPost("/{id:guid}/commands", async (Guid id, DispatchCommandRequest request, ISender sender) =>
        {
            var result = await sender.Send(new DispatchDeviceCommandCommand(id, request));
            return result.IsSuccess
                ? Results.Accepted($"/api/devices/{id}/commands/{result.Value.Id}", result.Value)
                : Results.BadRequest(result.Error);
        })
        .WithName("DispatchDeviceCommand")
        .WithSummary("Dispatch a Cloud-to-Device (C2D) command or Direct Method");

        group.MapGet("/{id:guid}/commands", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetDeviceCommandsQuery(id));
            return Results.Ok(result.Value);
        })
        .WithName("GetDeviceCommands")
        .WithSummary("Get command history for a device");

        group.MapPost("/{id:guid}/commands/{commandId:guid}/ack", async (Guid id, Guid commandId, bool success, string? error, ISender sender) =>
        {
            var result = await sender.Send(new AcknowledgeDeviceCommandCommand(id, commandId, success, error));
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
        })
        .WithName("AcknowledgeCommand")
        .WithSummary("Acknowledge command delivery/execution from device");

        return app;
    }
}
