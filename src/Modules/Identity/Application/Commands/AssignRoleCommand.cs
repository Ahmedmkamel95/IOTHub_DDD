using CIOT.Common.CQRS;
using CIOT.Common.Results;
using CIOT.Modules.Identity.Domain;
using CIOT.Modules.Identity.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.Identity.Application.Commands;

public record AssignRoleCommand(Guid UserId, string RoleName) : ICommand;

public class AssignRoleCommandHandler : IRequestHandler<AssignRoleCommand, Result>
{
    private readonly IdentityDbContext _dbContext;

    public AssignRoleCommandHandler(IdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(AssignRoleCommand command, CancellationToken cancellationToken)
    {
        var user = await _dbContext.UserAccounts
            .Include(u => u.RoleAssignments)
            .FirstOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);

        if (user == null)
            return Result.Failure(Error.NotFound("User.NotFound", $"User with ID '{command.UserId}' not found."));

        var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == command.RoleName, cancellationToken);
        if (role == null)
            return Result.Failure(Error.NotFound("Role.NotFound", $"Role '{command.RoleName}' does not exist."));

        if (!user.RoleAssignments.Any(ra => ra.RoleId == role.Id))
        {
            user.RoleAssignments.Add(new UserRoleAssignment { RoleId = role.Id, UserId = user.Id });
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return Result.Success();
    }
}
