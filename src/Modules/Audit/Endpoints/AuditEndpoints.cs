using CIOT.Modules.Audit.Application;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CIOT.Modules.Audit.Endpoints;

public static class AuditEndpoints
{
    public static IEndpointRouteBuilder MapAuditEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/audit").WithTags("Audit Trail");

        group.MapGet("/events", async (string? entityType, string? entityId, int? limit, ISender sender) =>
        {
            var result = await sender.Send(new GetAuditEventsQuery(entityType, entityId, limit ?? 50));
            return Results.Ok(result.Value);
        })
        .WithName("GetAuditEvents")
        .WithSummary("Query audit trail log events");

        return app;
    }
}
