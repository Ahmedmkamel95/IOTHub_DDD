using CIOT.Common.CQRS;
using CIOT.Common.Results;
using CIOT.Modules.Org.Application.Dtos;
using CIOT.Modules.Org.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.Org.Application.Queries;

public record GetCountriesQuery(bool ActiveOnly = true) : IQuery<List<CountryDto>>;
public record GetCountryByCodeQuery(string Code) : IQuery<CountryDto>;
public record GetBusinessUnitsQuery(string? CountryCode = null) : IQuery<List<BusinessUnitDto>>;
public record GetSalesOrganizationsQuery : IQuery<List<SalesOrganizationDto>>;

public class OrgQueryHandlers :
    IRequestHandler<GetCountriesQuery, Result<List<CountryDto>>>,
    IRequestHandler<GetCountryByCodeQuery, Result<CountryDto>>,
    IRequestHandler<GetBusinessUnitsQuery, Result<List<BusinessUnitDto>>>,
    IRequestHandler<GetSalesOrganizationsQuery, Result<List<SalesOrganizationDto>>>
{
    private readonly OrgDbContext _dbContext;

    public OrgQueryHandlers(OrgDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<CountryDto>>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Countries.AsNoTracking();
        if (request.ActiveOnly) query = query.Where(c => c.IsActive);

        var list = await query
            .OrderBy(c => c.CountryName)
            .Select(c => new CountryDto(c.CountryCode, c.CountryName, c.DefaultTimezone, c.IsActive))
            .ToListAsync(cancellationToken);

        return Result.Success(list);
    }

    public async Task<Result<CountryDto>> Handle(GetCountryByCodeQuery request, CancellationToken cancellationToken)
    {
        var country = await _dbContext.Countries.AsNoTracking()
            .FirstOrDefaultAsync(c => c.CountryCode.ToUpper() == request.Code.ToUpper(), cancellationToken);

        if (country == null)
            return Result.Failure<CountryDto>(Error.NotFound("Country.NotFound", $"Country with code '{request.Code}' not found."));

        return Result.Success(new CountryDto(country.CountryCode, country.CountryName, country.DefaultTimezone, country.IsActive));
    }

    public async Task<Result<List<BusinessUnitDto>>> Handle(GetBusinessUnitsQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.BusinessUnits.AsNoTracking();
        if (!string.IsNullOrEmpty(request.CountryCode))
        {
            query = query.Where(bu => bu.CountryCode.ToUpper() == request.CountryCode.ToUpper());
        }

        var list = await query
            .OrderBy(bu => bu.BusinessUnitCode)
            .Select(bu => new BusinessUnitDto(bu.Id, bu.BusinessUnitCode, bu.BusinessUnitName, bu.CountryCode, bu.IsActive))
            .ToListAsync(cancellationToken);

        return Result.Success(list);
    }

    public async Task<Result<List<SalesOrganizationDto>>> Handle(GetSalesOrganizationsQuery request, CancellationToken cancellationToken)
    {
        var list = await _dbContext.SalesOrganizations.AsNoTracking()
            .OrderBy(s => s.SalesOrganizationCode)
            .Select(s => new SalesOrganizationDto(s.Id, s.SalesOrganizationCode, s.DisplayName, s.CountryCode, s.IsActive))
            .ToListAsync(cancellationToken);

        return Result.Success(list);
    }
}
