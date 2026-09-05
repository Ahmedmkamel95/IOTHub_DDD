using System.Security.Claims;
using CIOT.Modules.Identity.Application.Commands;
using CIOT.Modules.Identity.Application.Dtos;
using CIOT.Modules.Identity.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CIOT.Modules.Identity.Endpoints;

public static class IdentityEndpoints
{
    public static IEndpointRouteBuilder MapIdentityEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/identity")
            .WithTags("Identity & Access");

        group.MapGet("/me", async (ClaimsPrincipal user, ISender sender) =>
        {
            var userIdClaim = user.FindFirst("user_id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Results.Unauthorized();
            }

            var result = await sender.Send(new GetUserByIdQuery(userId));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        })
        .WithName("GetCurrentUser")
        .WithSummary("Gets the profile and permissions of the currently authenticated user (Internal or External)");

        group.MapGet("/users", async (string? userType, int? page, int? pageSize, ISender sender) =>
        {
            var result = await sender.Send(new GetUsersQuery(userType, page ?? 1, pageSize ?? 20));
            return Results.Ok(result.Value);
        })
        .WithName("GetUsers")
        .WithSummary("List user accounts");

        group.MapGet("/users/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetUserByIdQuery(id));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        })
        .WithName("GetUserById")
        .WithSummary("Get user details by ID");

        group.MapPost("/users", async (CreateUserRequest request, ISender sender) =>
        {
            var result = await sender.Send(new CreateUserCommand(request));
            return result.IsSuccess
                ? Results.Created($"/api/identity/users/{result.Value.Id}", result.Value)
                : Results.BadRequest(result.Error);
        })
        .WithName("CreateUser")
        .WithSummary("Create or invite an internal or external user");

        group.MapPost("/users/{id:guid}/roles", async (Guid id, AssignRoleRequest request, ISender sender) =>
        {
            var result = await sender.Send(new AssignRoleCommand(id, request.RoleName));
            return result.IsSuccess ? Results.NoContent() : Results.BadRequest(result.Error);
        })
        .WithName("AssignRole")
        .WithSummary("Assign a role to a user");

        group.MapGet("/roles", async (ISender sender) =>
        {
            var result = await sender.Send(new GetRolesQuery());
            return Results.Ok(result.Value);
        })
        .WithName("GetRoles")
        .WithSummary("List all system roles and permissions");

        return app;
    }
}
