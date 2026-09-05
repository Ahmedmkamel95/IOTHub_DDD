using CIOT.Modules.Catalog.Application;
using CIOT.Modules.Catalog.Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CIOT.Modules.Catalog.Endpoints;

public static class CatalogEndpoints
{
    public static IEndpointRouteBuilder MapCatalogEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/catalog").WithTags("Catalog & Materials");

        group.MapGet("/materials", async (string? countryCode, ISender sender) =>
        {
            var result = await sender.Send(new GetMaterialsQuery(countryCode));
            return Results.Ok(result.Value);
        })
        .WithName("GetMaterials")
        .WithSummary("List catalog materials");

        group.MapPost("/materials", async (CreateMaterialRequest request, ISender sender) =>
        {
            var result = await sender.Send(new CreateMaterialCommand(request));
            return result.IsSuccess ? Results.Created($"/api/catalog/materials/{result.Value.Id}", result.Value) : Results.BadRequest(result.Error);
        })
        .WithName("CreateMaterial")
        .WithSummary("Register a new material");

        return app;
    }
}
