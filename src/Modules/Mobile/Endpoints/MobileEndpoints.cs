using CIOT.Modules.Mobile.Application;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CIOT.Modules.Mobile.Endpoints;

public static class MobileEndpoints
{
    public static IEndpointRouteBuilder MapMobileEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/mobile").WithTags("Mobile Field Operations");

        group.MapPost("/sync-batch", async (SyncOfflineBatchRequest request, ISender sender) =>
        {
            var result = await sender.Send(new SyncOfflineBatchCommand(request));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        })
        .WithName("SyncOfflineBatch")
        .WithSummary("Sync offline action batch performed by field technician app");

        return app;
    }
}
