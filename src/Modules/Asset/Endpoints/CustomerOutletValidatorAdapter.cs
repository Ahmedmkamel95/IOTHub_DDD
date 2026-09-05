using CIOT.Common.Results;
using CIOT.Modules.Asset.Application.Contracts;
using CIOT.Modules.CustomerOutlet.Endpoints;

namespace CIOT.Modules.Asset.Endpoints;

/// <summary>
/// Infrastructure / Endpoint adapter that implements the ICustomerOutletValidator port
/// by invoking the CustomerOutlet module's Endpoint Client across the module boundary.
/// </summary>
public sealed class CustomerOutletValidatorAdapter : ICustomerOutletValidator
{
    private readonly ICustomerOutletEndpointClient _client;

    public CustomerOutletValidatorAdapter(ICustomerOutletEndpointClient client)
    {
        _client = client;
    }

    public async Task<Result> ValidateCustomerAndClusterAsync(
        Guid customerId,
        Guid? outletId = null,
        Guid? clusterId = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _client.ValidateCustomerAndClusterAsync(customerId, outletId, clusterId, cancellationToken);
        if (!result.IsSuccess)
        {
            return Result.Failure(result.Error);
        }

        return Result.Success();
    }
}
