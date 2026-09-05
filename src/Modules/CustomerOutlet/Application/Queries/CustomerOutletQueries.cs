using CIOT.Common.CQRS;
using CIOT.Common.Results;
using CIOT.Modules.CustomerOutlet.Application.Dtos;
using CIOT.Modules.CustomerOutlet.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.CustomerOutlet.Application.Queries;

public record GetCustomersQuery(string? CountryCode = null, int Page = 1, int PageSize = 20) : IQuery<List<CustomerDto>>;
public record GetCustomerByIdQuery(Guid Id) : IQuery<CustomerDto>;
public record GetOutletsQuery(Guid? CustomerId = null, string? CountryCode = null, int Page = 1, int PageSize = 20) : IQuery<List<OutletDto>>;
public record GetOutletByIdQuery(Guid Id) : IQuery<OutletDto>;

public class CustomerOutletQueryHandlers :
    IRequestHandler<GetCustomersQuery, Result<List<CustomerDto>>>,
    IRequestHandler<GetCustomerByIdQuery, Result<CustomerDto>>,
    IRequestHandler<GetOutletsQuery, Result<List<OutletDto>>>,
    IRequestHandler<GetOutletByIdQuery, Result<OutletDto>>
{
    private readonly CustomerOutletDbContext _dbContext;

    public CustomerOutletQueryHandlers(CustomerOutletDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<CustomerDto>>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Customers.AsNoTracking();
        if (!string.IsNullOrEmpty(request.CountryCode))
        {
            query = query.Where(c => c.CountryCode.ToUpper() == request.CountryCode.ToUpper());
        }

        var list = await query
            .OrderBy(c => c.CustomerCode)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(c => new CustomerDto(c.Id, c.CustomerCode, c.CustomerName1, c.CustomerName2, c.CountryCode, c.VatNumber, c.IsActive))
            .ToListAsync(cancellationToken);

        return Result.Success(list);
    }

    public async Task<Result<CustomerDto>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var c = await _dbContext.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (c == null)
            return Result.Failure<CustomerDto>(Error.NotFound("Customer.NotFound", $"Customer '{request.Id}' not found."));

        return Result.Success(new CustomerDto(c.Id, c.CustomerCode, c.CustomerName1, c.CustomerName2, c.CountryCode, c.VatNumber, c.IsActive));
    }

    public async Task<Result<List<OutletDto>>> Handle(GetOutletsQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Outlets.AsNoTracking();
        if (request.CustomerId.HasValue) query = query.Where(o => o.CustomerId == request.CustomerId);
        if (!string.IsNullOrEmpty(request.CountryCode)) query = query.Where(o => o.CountryCode.ToUpper() == request.CountryCode.ToUpper());

        var list = await query
            .OrderBy(o => o.OutletCode)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(o => new OutletDto(o.Id, o.OutletCode, o.CustomerId, o.OutletType, o.AddressLine, o.City, o.PostalCode, o.CountryCode, o.Latitude, o.Longitude, o.SalesTerritoryId, o.IsActive))
            .ToListAsync(cancellationToken);

        return Result.Success(list);
    }

    public async Task<Result<OutletDto>> Handle(GetOutletByIdQuery request, CancellationToken cancellationToken)
    {
        var o = await _dbContext.Outlets.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (o == null)
            return Result.Failure<OutletDto>(Error.NotFound("Outlet.NotFound", $"Outlet '{request.Id}' not found."));

        return Result.Success(new OutletDto(o.Id, o.OutletCode, o.CustomerId, o.OutletType, o.AddressLine, o.City, o.PostalCode, o.CountryCode, o.Latitude, o.Longitude, o.SalesTerritoryId, o.IsActive));
    }
}
