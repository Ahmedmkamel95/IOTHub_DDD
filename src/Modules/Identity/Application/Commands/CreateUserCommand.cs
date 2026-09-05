using CIOT.Common.CQRS;
using CIOT.Common.Results;
using CIOT.Modules.Identity.Application.Dtos;
using CIOT.Modules.Identity.Domain;
using CIOT.Modules.Identity.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.Identity.Application.Commands;

public record CreateUserCommand(CreateUserRequest Request) : ICommand<UserDto>;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Request.Email).NotEmpty().EmailAddress().MaximumLength(255);
        RuleFor(x => x.Request.UserType).NotEmpty().Must(t => t == "Internal" || t == "External")
            .WithMessage("UserType must be 'Internal' or 'External'");
    }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserDto>>
{
    private readonly IdentityDbContext _dbContext;

    public CreateUserCommandHandler(IdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<UserDto>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var req = command.Request;
        var existing = await _dbContext.UserAccounts.AnyAsync(u => u.Email.ToLower() == req.Email.ToLower(), cancellationToken);
        if (existing)
        {
            return Result.Failure<UserDto>(Error.Conflict("User.DuplicateEmail", $"A user with email '{req.Email}' already exists."));
        }

        var user = new UserAccount
        {
            Email = req.Email.ToLowerInvariant(),
            DisplayName = req.DisplayName ?? $"{req.FirstName} {req.LastName}".Trim(),
            FirstName = req.FirstName,
            LastName = req.LastName,
            UserType = req.UserType,
            MainCountryCode = req.MainCountryCode,
            AuthProvider = req.UserType == "Internal" ? "EntraID" : "EntraExternalId",
            Status = "Invited"
        };

        if (req.RoleNames != null && req.RoleNames.Count != 0)
        {
            var roles = await _dbContext.Roles
                .Where(r => req.RoleNames.Contains(r.Name))
                .ToListAsync(cancellationToken);

            foreach (var role in roles)
            {
                user.RoleAssignments.Add(new UserRoleAssignment { Role = role, UserAccount = user });
            }
        }

        _dbContext.UserAccounts.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

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
}
