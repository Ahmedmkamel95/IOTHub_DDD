using CIOT.Common.Results;
using CIOT.Modules.CustomerOutlet.Application.Queries;
using MediatR;

namespace CIOT.Modules.CustomerOutlet.Endpoints;

public record CustomerValidationResponse(
    bool IsValid,
    Guid CustomerId,
    Guid? ClusterId,
    Guid? OutletId,
    string? Message = null);

public interface ICustomerOutletEndpointClient
{
    Task<Result<CustomerValidationResponse>> ValidateCustomerAndClusterAsync(
        Guid customerId,
        Guid? outletId = null,
        Guid? clusterId = null,
        CancellationToken cancellationToken = default);
}

public sealed class CustomerOutletEndpointClient : ICustomerOutletEndpointClient
{
    private readonly ISender _sender;

    public CustomerOutletEndpointClient(ISender sender)
    {
        _sender = sender;
    }

    public async Task<Result<CustomerValidationResponse>> ValidateCustomerAndClusterAsync(
        Guid customerId,
        Guid? outletId = null,
        Guid? clusterId = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new ValidateCustomerClusterQuery(customerId, outletId, clusterId), cancellationToken);
        if (!result.IsSuccess)
        {
            return Result.Failure<CustomerValidationResponse>(result.Error);
        }

        return Result.Success(new CustomerValidationResponse(
            result.Value.IsValid,
            result.Value.CustomerId,
            result.Value.ClusterId,
            result.Value.OutletId,
            result.Value.Message));
    }
}
