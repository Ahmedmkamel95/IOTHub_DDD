using CIOT.Modules.Org.Application.Commands;
using CIOT.Modules.Org.Application.Dtos;
using CIOT.Modules.Org.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CIOT.Modules.Org.Endpoints;

public static class OrgEndpoints
{
    public static IEndpointRouteBuilder MapOrgEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/org")
            .WithTags("Organization");

        // Countries
        group.MapGet("/countries", async (bool? activeOnly, ISender sender) =>
        {
            var result = await sender.Send(new GetCountriesQuery(activeOnly ?? true));
            return Results.Ok(result.Value);
        })
        .WithName("GetCountries")
        .WithSummary("List registered countries");

        group.MapGet("/countries/{code}", async (string code, ISender sender) =>
        {
            var result = await sender.Send(new GetCountryByCodeQuery(code));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        })
        .WithName("GetCountryByCode")
        .WithSummary("Get country by ISO code");

        group.MapPost("/countries", async (CreateCountryRequest request, ISender sender) =>
        {
            var result = await sender.Send(new CreateCountryCommand(request));
            return result.IsSuccess
                ? Results.Created($"/api/org/countries/{result.Value.CountryCode}", result.Value)
                : Results.BadRequest(result.Error);
        })
        .WithName("CreateCountry")
        .WithSummary("Register a new country");

        // Business Units
        group.MapGet("/business-units", async (string? countryCode, ISender sender) =>
        {
            var result = await sender.Send(new GetBusinessUnitsQuery(countryCode));
            return Results.Ok(result.Value);
        })
        .WithName("GetBusinessUnits")
        .WithSummary("List business units");

        group.MapPost("/business-units", async (CreateBusinessUnitRequest request, ISender sender) =>
        {
            var result = await sender.Send(new CreateBusinessUnitCommand(request));
            return result.IsSuccess
                ? Results.Created($"/api/org/business-units/{result.Value.Id}", result.Value)
                : Results.BadRequest(result.Error);
        })
        .WithName("CreateBusinessUnit")
        .WithSummary("Create a new business unit");

        // Sales Organizations
        group.MapGet("/sales-organizations", async (ISender sender) =>
        {
            var result = await sender.Send(new GetSalesOrganizationsQuery());
            return Results.Ok(result.Value);
        })
        .WithName("GetSalesOrganizations")
        .WithSummary("List sales organizations");

        group.MapPost("/sales-organizations", async (CreateSalesOrganizationRequest request, ISender sender) =>
        {
            var result = await sender.Send(new CreateSalesOrganizationCommand(request));
            return result.IsSuccess
                ? Results.Created($"/api/org/sales-organizations/{result.Value.Id}", result.Value)
                : Results.BadRequest(result.Error);
        })
        .WithName("CreateSalesOrganization")
        .WithSummary("Create a new sales organization");

        return app;
    }
}
