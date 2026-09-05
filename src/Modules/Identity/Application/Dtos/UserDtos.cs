namespace CIOT.Modules.Identity.Application.Dtos;

public record UserDto(
    Guid Id,
    string Email,
    string? DisplayName,
    string? FirstName,
    string? LastName,
    string UserType,
    string Status,
    string? AuthProvider,
    string? ExternalIdentityId,
    List<string> Roles,
    DateTime CreatedAtUtc,
    DateTime? LastLoginAtUtc
);

public record CreateUserRequest(
    string Email,
    string? DisplayName,
    string? FirstName,
    string? LastName,
    string UserType,
    string? MainCountryCode,
    List<string>? RoleNames
);

public record RoleDto(Guid Id, string Name, string? Description, List<string> Permissions);
public record AssignRoleRequest(string RoleName);
