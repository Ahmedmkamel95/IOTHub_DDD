using CIOT.Common.CQRS;
using CIOT.Common.Results;
using CIOT.Modules.CustomerOutlet.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.CustomerOutlet.Application.Queries;

public record CustomerValidationResult(
    bool IsValid,
    Guid CustomerId,
    Guid? ClusterId,
    Guid? OutletId,
    string? Message = null);

public record ValidateCustomerClusterQuery(
    Guid CustomerId,
    Guid? OutletId = null,
    Guid? ClusterId = null) : IQuery<CustomerValidationResult>;

public class ValidateCustomerClusterQueryHandler : IRequestHandler<ValidateCustomerClusterQuery, Result<CustomerValidationResult>>
{
    private readonly CustomerOutletDbContext _dbContext;

    public ValidateCustomerClusterQueryHandler(CustomerOutletDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<CustomerValidationResult>> Handle(ValidateCustomerClusterQuery query, CancellationToken cancellationToken)
    {
        // 1. Validate Customer
        var customer = await _dbContext.Customers
            .Include(c => c.CustomerCluster)
            .FirstOrDefaultAsync(c => c.Id == query.CustomerId, cancellationToken);

        if (customer == null)
        {
            return Result.Failure<CustomerValidationResult>(
                Error.NotFound("Customer.NotFound", $"Customer with ID '{query.CustomerId}' was not found."));
        }

        if (!customer.IsActive)
        {
            return Result.Failure<CustomerValidationResult>(
                Error.Validation("Customer.Inactive", $"Customer '{customer.CustomerCode}' is inactive."));
        }

        // 2. Validate Customer Cluster
        Guid effectiveClusterId;
        if (query.ClusterId.HasValue)
        {
            effectiveClusterId = query.ClusterId.Value;
            var cluster = await _dbContext.CustomerClusters
                .FirstOrDefaultAsync(cc => cc.Id == effectiveClusterId, cancellationToken);

            if (cluster == null)
            {
                return Result.Failure<CustomerValidationResult>(
                    Error.NotFound("Cluster.NotFound", $"Customer cluster '{effectiveClusterId}' was not found."));
            }

            if (!cluster.IsActive)
            {
                return Result.Failure<CustomerValidationResult>(
                    Error.Validation("Cluster.Inactive", $"Customer cluster '{cluster.ClusterCode}' is inactive."));
            }

            if (customer.CustomerClusterId.HasValue && customer.CustomerClusterId.Value != effectiveClusterId)
            {
                return Result.Failure<CustomerValidationResult>(
                    Error.Validation("Customer.ClusterMismatch", $"Customer belongs to cluster '{customer.CustomerClusterId}' instead of requested cluster '{effectiveClusterId}'."));
            }
        }
        else
        {
            if (!customer.CustomerClusterId.HasValue || customer.CustomerCluster == null)
            {
                return Result.Failure<CustomerValidationResult>(
                    Error.Validation("Customer.NoCluster", $"Customer '{customer.CustomerCode}' is not assigned to any cluster."));
            }

            if (!customer.CustomerCluster.IsActive)
            {
                return Result.Failure<CustomerValidationResult>(
                    Error.Validation("Cluster.Inactive", $"Customer cluster '{customer.CustomerCluster.ClusterCode}' is inactive."));
            }

            effectiveClusterId = customer.CustomerClusterId.Value;
        }

        // 3. Validate Outlet (if provided)
        if (query.OutletId.HasValue)
        {
            var outlet = await _dbContext.Outlets
                .FirstOrDefaultAsync(o => o.Id == query.OutletId.Value, cancellationToken);

            if (outlet == null)
            {
                return Result.Failure<CustomerValidationResult>(
                    Error.NotFound("Outlet.NotFound", $"Outlet with ID '{query.OutletId.Value}' was not found."));
            }

            if (!outlet.IsActive)
            {
                return Result.Failure<CustomerValidationResult>(
                    Error.Validation("Outlet.Inactive", $"Outlet '{outlet.OutletCode}' is inactive."));
            }

            if (outlet.CustomerId.HasValue && outlet.CustomerId.Value != customer.Id)
            {
                return Result.Failure<CustomerValidationResult>(
                    Error.Validation("Outlet.CustomerMismatch", $"Outlet '{outlet.OutletCode}' is assigned to another customer '{outlet.CustomerId.Value}'."));
            }
        }

        return Result.Success(new CustomerValidationResult(
            IsValid: true,
            CustomerId: customer.Id,
            ClusterId: effectiveClusterId,
            OutletId: query.OutletId,
            Message: "Customer, cluster, and outlet are valid and active."));
    }
}
