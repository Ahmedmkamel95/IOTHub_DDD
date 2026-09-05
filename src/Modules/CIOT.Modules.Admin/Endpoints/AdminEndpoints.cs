using CIOT.Modules.Admin.Application;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CIOT.Modules.Admin.Endpoints;

public static class AdminEndpoints
{
 
    public static IEndpointRouteBuilder MapAdminEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/admin").WithTags("Admin & Policies");

        group.MapGet("/equipment-models", async (ISender sender) =>
        {
            var result = await sender.Send(new GetEquipmentModelsQuery());
            return Results.Ok(result.Value);
        })
        .WithName("GetEquipmentModels")
        .WithSummary("List equipment models");

        group.MapPost("/equipment-models", async (CreateEquipmentModelRequest request, ISender sender) =>
        {
            var result = await sender.Send(new CreateEquipmentModelCommand(request));
            return result.IsSuccess ? Results.Created($"/api/admin/equipment-models/{result.Value.Id}", result.Value) : Results.BadRequest(result.Error);
        })
        .WithName("CreateEquipmentModel")
        .WithSummary("Register a new equipment model");

        return app;
    }
}
