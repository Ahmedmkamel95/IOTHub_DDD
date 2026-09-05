using CIOT.Common.Results;

namespace CIOT.Common.Contracts.CustomerOutlet;

public record CustomerValidationResponse(
    bool IsValid,
    Guid CustomerId,
    Guid? ClusterId,
    Guid? OutletId,
    string? Message = null);

public interface ICustomerOutletApi
{
    Task<Result<CustomerValidationResponse>> ValidateCustomerAndClusterAsync(
        Guid customerId,
        Guid? outletId = null,
        Guid? clusterId = null,
        CancellationToken cancellationToken = default);
}
