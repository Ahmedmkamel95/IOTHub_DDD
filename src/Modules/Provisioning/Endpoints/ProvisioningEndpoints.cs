using CIOT.Modules.Provisioning.Application.Commands;
using CIOT.Modules.Provisioning.Application.Dtos;
using CIOT.Modules.Provisioning.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CIOT.Modules.Provisioning.Endpoints;

public static class ProvisioningEndpoints
{
    public static IEndpointRouteBuilder MapProvisioningEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/provisioning")
            .WithTags("Provisioning & Hardware");

        group.MapGet("/manufacturers", async (ISender sender) =>
        {
            var result = await sender.Send(new GetDeviceManufacturersQuery());
            return Results.Ok(result.Value);
        })
        .WithName("GetDeviceManufacturers")
        .WithSummary("List hardware manufacturers");

        group.MapPost("/manufacturers", async (CreateDeviceManufacturerRequest request, ISender sender) =>
        {
            var result = await sender.Send(new CreateDeviceManufacturerCommand(request));
            return result.IsSuccess
                ? Results.Created($"/api/provisioning/manufacturers/{result.Value.Id}", result.Value)
                : Results.BadRequest(result.Error);
        })
        .WithName("CreateDeviceManufacturer")
        .WithSummary("Register a hardware manufacturer");

        group.MapGet("/models", async (Guid? manufacturerId, ISender sender) =>
        {
            var result = await sender.Send(new GetDeviceModelsQuery(manufacturerId));
            return Results.Ok(result.Value);
        })
        .WithName("GetDeviceModels")
        .WithSummary("List device hardware models");

        group.MapPost("/models", async (CreateDeviceModelRequest request, ISender sender) =>
        {
            var result = await sender.Send(new CreateDeviceModelCommand(request));
            return result.IsSuccess
                ? Results.Created($"/api/provisioning/models/{result.Value.Id}", result.Value)
                : Results.BadRequest(result.Error);
        })
        .WithName("CreateDeviceModel")
        .WithSummary("Register a device hardware model");

        return app;
    }
}
