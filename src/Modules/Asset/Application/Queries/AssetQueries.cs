using CIOT.Common.CQRS;
using CIOT.Common.Results;
using CIOT.Modules.Asset.Application.Dtos;
using CIOT.Modules.Asset.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.Asset.Application.Queries;

public record GetAssetsQuery(string? CountryCode = null, int Page = 1, int PageSize = 20) : IQuery<List<AssetDto>>;
public record GetAssetByIdQuery(Guid Id) : IQuery<AssetDto>;
public record GetAssetAssignmentsQuery(Guid AssetId) : IQuery<List<AssetOutletAssignmentDto>>;

public class AssetQueryHandlers :
    IRequestHandler<GetAssetsQuery, Result<List<AssetDto>>>,
    IRequestHandler<GetAssetByIdQuery, Result<AssetDto>>,
    IRequestHandler<GetAssetAssignmentsQuery, Result<List<AssetOutletAssignmentDto>>>
{
    private readonly AssetDbContext _dbContext;

    public AssetQueryHandlers(AssetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<AssetDto>>> Handle(GetAssetsQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Assets
            .AsNoTracking()
            .Include(a => a.OutletAssignments)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.CountryCode))
        {
            query = query.Where(a => a.CountryCode.ToUpper() == request.CountryCode.ToUpper());
        }

        var list = await query
            .OrderBy(a => a.SapEquipmentNumber)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(a => new AssetDto(
                a.Id,
                a.SapEquipmentNumber,
                a.OemSerialNumber,
                a.TechnicalId,
                a.EquipmentModelId,
                a.CountryCode,
                a.SapStatus,
                a.IsActive,
                a.OutletAssignments.Where(oa => oa.IsCurrent).Select(oa => oa.OutletId).FirstOrDefault(),
                a.LastConnectionAtUtc
            ))
            .ToListAsync(cancellationToken);

        return Result.Success(list);
    }

    public async Task<Result<AssetDto>> Handle(GetAssetByIdQuery request, CancellationToken cancellationToken)
    {
        var a = await _dbContext.Assets
            .AsNoTracking()
            .Include(x => x.OutletAssignments)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (a == null)
            return Result.Failure<AssetDto>(Error.NotFound("Asset.NotFound", $"Asset '{request.Id}' not found."));

        var dto = new AssetDto(
            a.Id,
            a.SapEquipmentNumber,
            a.OemSerialNumber,
            a.TechnicalId,
            a.EquipmentModelId,
            a.CountryCode,
            a.SapStatus,
            a.IsActive,
            a.OutletAssignments.Where(oa => oa.IsCurrent).Select(oa => oa.OutletId).FirstOrDefault(),
            a.LastConnectionAtUtc
        );

        return Result.Success(dto);
    }

    public async Task<Result<List<AssetOutletAssignmentDto>>> Handle(GetAssetAssignmentsQuery request, CancellationToken cancellationToken)
    {
        var list = await _dbContext.AssetOutletAssignments
            .AsNoTracking()
            .Where(oa => oa.AssetId == request.AssetId)
            .OrderByDescending(oa => oa.AssignedAtUtc)
            .Select(oa => new AssetOutletAssignmentDto(
                oa.Id,
                oa.AssetId,
                oa.OutletId,
                oa.CustomerId,
                oa.AssignedAtUtc,
                oa.UnassignedAtUtc,
                oa.IsCurrent
            ))
            .ToListAsync(cancellationToken);

        return Result.Success(list);
    }
}
