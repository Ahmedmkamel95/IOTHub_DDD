using CIOT.Modules.Report.Application;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CIOT.Modules.Report.Endpoints;

public static class ReportEndpoints
{
    public static IEndpointRouteBuilder MapReportEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/reports").WithTags("Reporting & Exports");

        group.MapGet("/definitions", async (ISender sender) =>
        {
            var result = await sender.Send(new GetReportDefinitionsQuery());
            return Results.Ok(result.Value);
        })
        .WithName("GetReportDefinitions")
        .WithSummary("List report definitions");

        group.MapPost("/definitions", async (CreateReportDefinitionRequest request, ISender sender) =>
        {
            var result = await sender.Send(new CreateReportDefinitionCommand(request));
            return result.IsSuccess ? Results.Created($"/api/reports/definitions/{result.Value.Id}", result.Value) : Results.BadRequest(result.Error);
        })
        .WithName("CreateReportDefinition")
        .WithSummary("Create a new report definition");

        return app;
    }
}
