using CIOT.Common.Results;

namespace CIOT.Modules.Asset.Application.Contracts;

/// <summary>
/// Port (Light DDD) in Asset.Application for validating customer and cluster rules via the CustomerOutlet module boundary.
/// </summary>
public interface ICustomerOutletValidator
{
    Task<Result> ValidateCustomerAndClusterAsync(
        Guid customerId,
        Guid? outletId = null,
        Guid? clusterId = null,
        CancellationToken cancellationToken = default);
}
