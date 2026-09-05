using CIOT.Modules.LocalAdapter.Application;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CIOT.Modules.LocalAdapter.Endpoints;

public static class LocalAdapterEndpoints
{
    public static IEndpointRouteBuilder MapLocalAdapterEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/local-adapter").WithTags("Local Adapter & Edge");

        group.MapGet("/devices/{deviceId:guid}/effects", async (Guid deviceId, ISender sender) =>
        {
            var result = await sender.Send(new GetDeviceEffectsQuery(deviceId));
            return Results.Ok(result.Value);
        })
        .WithName("GetDeviceEffects")
        .WithSummary("Get edge projection effects applied to a device");

        return app;
    }
}
