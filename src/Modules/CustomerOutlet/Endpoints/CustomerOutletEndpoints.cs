using CIOT.Common.Contracts.CustomerOutlet;
using CIOT.Modules.CustomerOutlet.Application.Commands;
using CIOT.Modules.CustomerOutlet.Application.Dtos;
using CIOT.Modules.CustomerOutlet.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CIOT.Modules.CustomerOutlet.Endpoints;

public static class CustomerOutletEndpoints
{
    public static IEndpointRouteBuilder MapCustomerOutletEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/customer-outlets")
            .WithTags("Customer & Outlet");

        // Clusters
        group.MapPost("/clusters", async (CreateCustomerClusterRequest request, ISender sender) =>
        {
            var result = await sender.Send(new CreateCustomerClusterCommand(request));
            return result.IsSuccess
                ? Results.Created($"/api/customer-outlets/clusters/{result.Value.Id}", result.Value)
                : Results.BadRequest(result.Error);
        })
        .WithName("CreateCustomerCluster")
        .WithSummary("Create a new customer cluster");

        // Customers
        group.MapGet("/customers", async (string? countryCode, int? page, int? pageSize, ISender sender) =>
        {
            var result = await sender.Send(new GetCustomersQuery(countryCode, page ?? 1, pageSize ?? 20));
            return Results.Ok(result.Value);
        })
        .WithName("GetCustomers")
        .WithSummary("List customers");

        group.MapGet("/customers/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetCustomerByIdQuery(id));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        })
        .WithName("GetCustomerById")
        .WithSummary("Get customer by ID");

        group.MapPost("/customers", async (CreateCustomerRequest request, ISender sender) =>
        {
            var result = await sender.Send(new CreateCustomerCommand(request));
            return result.IsSuccess
                ? Results.Created($"/api/customer-outlets/customers/{result.Value.Id}", result.Value)
                : Results.BadRequest(result.Error);
        })
        .WithName("CreateCustomer")
        .WithSummary("Create a new customer");

        group.MapGet("/customers/{id:guid}/validate-cluster", async (Guid id, Guid? outletId, Guid? clusterId, ICustomerOutletApi client, CancellationToken ct) =>
        {
            var result = await client.ValidateCustomerAndClusterAsync(id, outletId, clusterId, ct);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        })
        .WithName("ValidateCustomerAndCluster")
        .WithSummary("Validate customer active status, cluster active status, and outlet assignment");

        // Outlets
        group.MapGet("/outlets", async (Guid? customerId, string? countryCode, int? page, int? pageSize, ISender sender) =>
        {
            var result = await sender.Send(new GetOutletsQuery(customerId, countryCode, page ?? 1, pageSize ?? 20));
            return Results.Ok(result.Value);
        })
        .WithName("GetOutlets")
        .WithSummary("List outlets");

        group.MapGet("/outlets/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetOutletByIdQuery(id));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        })
        .WithName("GetOutletById")
        .WithSummary("Get outlet by ID");

        group.MapPost("/outlets", async (CreateOutletRequest request, ISender sender) =>
        {
            var result = await sender.Send(new CreateOutletCommand(request));
            return result.IsSuccess
                ? Results.Created($"/api/customer-outlets/outlets/{result.Value.Id}", result.Value)
                : Results.BadRequest(result.Error);
        })
        .WithName("CreateOutlet")
        .WithSummary("Create a new outlet");

        group.MapPost("/outlets/{id:guid}/notes", async (Guid id, AddOutletNoteRequest request, ISender sender) =>
        {
            var result = await sender.Send(new AddOutletNoteCommand(id, request));
            return result.IsSuccess ? Results.Created($"/api/customer-outlets/outlets/{id}/notes", result.Value) : Results.BadRequest(result.Error);
        })
        .WithName("AddOutletNote")
        .WithSummary("Add a note to an outlet");

        return app;
    }
}
