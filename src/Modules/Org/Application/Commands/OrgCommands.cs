using CIOT.Common.CQRS;
using CIOT.Common.Results;
using CIOT.Modules.Org.Application.Dtos;
using CIOT.Modules.Org.Domain;
using CIOT.Modules.Org.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.Org.Application.Commands;

public record CreateCountryCommand(CreateCountryRequest Request) : ICommand<CountryDto>;
public record CreateBusinessUnitCommand(CreateBusinessUnitRequest Request) : ICommand<BusinessUnitDto>;
public record CreateSalesOrganizationCommand(CreateSalesOrganizationRequest Request) : ICommand<SalesOrganizationDto>;

public class CreateCountryCommandValidator : AbstractValidator<CreateCountryCommand>
{
    public CreateCountryCommandValidator()
    {
        RuleFor(x => x.Request.CountryCode).NotEmpty().MaximumLength(10);
        RuleFor(x => x.Request.CountryName).NotEmpty().MaximumLength(150);
    }
}

public class OrgCommandHandlers :
    IRequestHandler<CreateCountryCommand, Result<CountryDto>>,
    IRequestHandler<CreateBusinessUnitCommand, Result<BusinessUnitDto>>,
    IRequestHandler<CreateSalesOrganizationCommand, Result<SalesOrganizationDto>>
{
    private readonly OrgDbContext _dbContext;

    public OrgCommandHandlers(OrgDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<CountryDto>> Handle(CreateCountryCommand command, CancellationToken cancellationToken)
    {
        var req = command.Request;
        var existing = await _dbContext.Countries.AnyAsync(c => c.CountryCode.ToUpper() == req.CountryCode.ToUpper(), cancellationToken);
        if (existing)
        {
            return Result.Failure<CountryDto>(Error.Conflict("Country.Duplicate", $"Country code '{req.CountryCode}' already exists."));
        }

        var country = new Country
        {
            CountryCode = req.CountryCode.ToUpperInvariant(),
            CountryName = req.CountryName,
            DefaultTimezone = req.DefaultTimezone,
            IsActive = true
        };

        _dbContext.Countries.Add(country);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new CountryDto(country.CountryCode, country.CountryName, country.DefaultTimezone, country.IsActive));
    }

    public async Task<Result<BusinessUnitDto>> Handle(CreateBusinessUnitCommand command, CancellationToken cancellationToken)
    {
        var req = command.Request;
        var countryExists = await _dbContext.Countries.AnyAsync(c => c.CountryCode.ToUpper() == req.CountryCode.ToUpper(), cancellationToken);
        if (!countryExists)
        {
            return Result.Failure<BusinessUnitDto>(Error.NotFound("Country.NotFound", $"Country '{req.CountryCode}' does not exist."));
        }

        var bu = new BusinessUnit
        {
            BusinessUnitCode = req.BusinessUnitCode.ToUpperInvariant(),
            BusinessUnitName = req.BusinessUnitName,
            CountryCode = req.CountryCode.ToUpperInvariant(),
            IsActive = true
        };

        _dbContext.BusinessUnits.Add(bu);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new BusinessUnitDto(bu.Id, bu.BusinessUnitCode, bu.BusinessUnitName, bu.CountryCode, bu.IsActive));
    }

    public async Task<Result<SalesOrganizationDto>> Handle(CreateSalesOrganizationCommand command, CancellationToken cancellationToken)
    {
        var req = command.Request;
        var so = new SalesOrganization
        {
            SalesOrganizationCode = req.SalesOrganizationCode.ToUpperInvariant(),
            DisplayName = req.DisplayName,
            CountryCode = req.CountryCode?.ToUpperInvariant(),
            IsActive = true
        };

        _dbContext.SalesOrganizations.Add(so);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new SalesOrganizationDto(so.Id, so.SalesOrganizationCode, so.DisplayName, so.CountryCode, so.IsActive));
    }
}
