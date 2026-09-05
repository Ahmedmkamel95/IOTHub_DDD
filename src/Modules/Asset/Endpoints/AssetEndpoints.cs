using CIOT.Modules.Asset.Application.Commands;
using CIOT.Modules.Asset.Application.Dtos;
using CIOT.Modules.Asset.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CIOT.Modules.Asset.Endpoints;

public static class AssetEndpoints
{
    public static IEndpointRouteBuilder MapAssetEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/assets")
            .WithTags("Asset Management");

        group.MapGet("/", async (string? countryCode, int? page, int? pageSize, ISender sender) =>
        {
            var result = await sender.Send(new GetAssetsQuery(countryCode, page ?? 1, pageSize ?? 20));
            return Results.Ok(result.Value);
        })
        .WithName("GetAssets")
        .WithSummary("List assets");

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetAssetByIdQuery(id));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        })
        .WithName("GetAssetById")
        .WithSummary("Get asset by ID");

        group.MapPost("/", async (RegisterAssetRequest request, ISender sender) =>
        {
            var result = await sender.Send(new RegisterAssetCommand(request));
            return result.IsSuccess
                ? Results.Created($"/api/assets/{result.Value.Id}", result.Value)
                : Results.BadRequest(result.Error);
        })
        .WithName("RegisterAsset")
        .WithSummary("Register a new asset");

        group.MapPost("/{id:guid}/assign-outlet", async (Guid id, AssignAssetToOutletRequest request, ISender sender) =>
        {
            var result = await sender.Send(new AssignAssetToOutletCommand(id, request));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        })
        .WithName("AssignAssetToOutlet")
        .WithSummary("Assign an asset to an outlet");

        group.MapPost("/{id:guid}/assign-customer-outlet", async (Guid id, AssignAssetToCustomerOutletRequest request, ISender sender) =>
        {
            var result = await sender.Send(new AssignAssetToCustomerOutletCommand(id, request));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        })
        .WithName("AssignAssetToCustomerOutlet")
        .WithSummary("Assign an asset to a customer and outlet with cluster validation");

        group.MapGet("/{id:guid}/assignments", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetAssetAssignmentsQuery(id));
            return Results.Ok(result.Value);
        })
        .WithName("GetAssetAssignments")
        .WithSummary("Get assignment history of an asset");

        return app;
    }
}
