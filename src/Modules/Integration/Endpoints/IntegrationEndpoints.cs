using CIOT.Modules.Integration.Application;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CIOT.Modules.Integration.Endpoints;

public static class IntegrationEndpoints
{
    public static IEndpointRouteBuilder MapIntegrationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/integration").WithTags("Integration & Partners");

        group.MapGet("/partners", async (ISender sender) =>
        {
            var result = await sender.Send(new GetPartnerSourcesQuery());
            return Results.Ok(result.Value);
        })
        .WithName("GetPartnerSources")
        .WithSummary("List external partner data sources");

        group.MapPost("/partners", async (CreatePartnerSourceRequest request, ISender sender) =>
        {
            var result = await sender.Send(new CreatePartnerSourceCommand(request));
            return result.IsSuccess ? Results.Created($"/api/integration/partners/{result.Value.Id}", result.Value) : Results.BadRequest(result.Error);
        })
        .WithName("CreatePartnerSource")
        .WithSummary("Register a partner integration source");

        return app;
    }
}
