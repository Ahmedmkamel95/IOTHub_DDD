using CIOT.Common.CQRS;
using CIOT.Common.Results;
using CIOT.Modules.Identity.Application.Dtos;
using CIOT.Modules.Identity.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.Identity.Application.Queries;

public record GetUserByIdQuery(Guid UserId) : IQuery<UserDto>;
public record GetUsersQuery(string? UserType = null, int Page = 1, int PageSize = 20) : IQuery<List<UserDto>>;
public record GetRolesQuery : IQuery<List<RoleDto>>;

public class IdentityQueryHandlers :
    IRequestHandler<GetUserByIdQuery, Result<UserDto>>,
    IRequestHandler<GetUsersQuery, Result<List<UserDto>>>,
    IRequestHandler<GetRolesQuery, Result<List<RoleDto>>>
{
    private readonly IdentityDbContext _dbContext;

    public IdentityQueryHandlers(IdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.UserAccounts
            .AsNoTracking()
            .Include(u => u.RoleAssignments).ThenInclude(ra => ra.Role)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
            return Result.Failure<UserDto>(Error.NotFound("User.NotFound", $"User with ID '{request.UserId}' not found."));

        var dto = new UserDto(
            user.Id,
            user.Email,
            user.DisplayName,
            user.FirstName,
            user.LastName,
            user.UserType,
            user.Status,
            user.AuthProvider,
            user.ExternalIdentityId,
            user.RoleAssignments.Select(ra => ra.Role.Name).ToList(),
            user.CreatedAtUtc,
            user.LastLoginAtUtc
        );

        return Result.Success(dto);
    }

    public async Task<Result<List<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.UserAccounts
            .AsNoTracking()
            .Include(u => u.RoleAssignments).ThenInclude(ra => ra.Role)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.UserType))
        {
            query = query.Where(u => u.UserType == request.UserType);
        }

        var users = await query
            .OrderByDescending(u => u.CreatedAtUtc)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(user => new UserDto(
                user.Id,
                user.Email,
                user.DisplayName,
                user.FirstName,
                user.LastName,
                user.UserType,
                user.Status,
                user.AuthProvider,
                user.ExternalIdentityId,
                user.RoleAssignments.Select(ra => ra.Role.Name).ToList(),
                user.CreatedAtUtc,
                user.LastLoginAtUtc
            ))
            .ToListAsync(cancellationToken);

        return Result.Success(users);
    }

    public async Task<Result<List<RoleDto>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _dbContext.Roles
            .AsNoTracking()
            .Include(r => r.RolePermissions).ThenInclude(rp => rp.Permission)
            .Select(r => new RoleDto(
                r.Id,
                r.Name,
                r.Description,
                r.RolePermissions.Select(rp => rp.Permission.Code).ToList()
            ))
            .ToListAsync(cancellationToken);

        return Result.Success(roles);
    }
}
