using CIOT.Common.CQRS;
using CIOT.Common.Results;
using CIOT.Modules.Catalog.Application.Dtos;
using CIOT.Modules.Catalog.Domain;
using CIOT.Modules.Catalog.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.Catalog.Application;

public record CreateMaterialCommand(CreateMaterialRequest Request) : ICommand<MaterialDto>;
public record GetMaterialsQuery(string? CountryCode = null) : IQuery<List<MaterialDto>>;

public class CatalogHandlers :
    IRequestHandler<CreateMaterialCommand, Result<MaterialDto>>,
    IRequestHandler<GetMaterialsQuery, Result<List<MaterialDto>>>
{
    private readonly CatalogDbContext _dbContext;

    public CatalogHandlers(CatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<MaterialDto>> Handle(CreateMaterialCommand command, CancellationToken cancellationToken)
    {
        var req = command.Request;
        var existing = await _dbContext.Materials.AnyAsync(m => m.MaterialCode == req.MaterialCode && m.CountryCode == req.CountryCode, cancellationToken);
        if (existing)
            return Result.Failure<MaterialDto>(Error.Conflict("Material.Duplicate", $"Material '{req.MaterialCode}' already exists in country '{req.CountryCode}'."));

        var material = new Material
        {
            MaterialCode = req.MaterialCode,
            ProductName = req.ProductName,
            CountryCode = req.CountryCode.ToUpperInvariant(),
            BusinessUnitId = req.BusinessUnitId,
            IsActive = true
        };

        _dbContext.Materials.Add(material);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new MaterialDto(material.Id, material.MaterialCode, material.ProductName, material.CountryCode, material.IsActive));
    }

    public async Task<Result<List<MaterialDto>>> Handle(GetMaterialsQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Materials.AsNoTracking();
        if (!string.IsNullOrEmpty(request.CountryCode))
            query = query.Where(m => m.CountryCode.ToUpper() == request.CountryCode.ToUpper());

        var list = await query
            .OrderBy(m => m.MaterialCode)
            .Select(m => new MaterialDto(m.Id, m.MaterialCode, m.ProductName, m.CountryCode, m.IsActive))
            .ToListAsync(cancellationToken);

        return Result.Success(list);
    }
}
