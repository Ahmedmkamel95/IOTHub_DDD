using System.Security.Claims;
using CIOT.Modules.Identity.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CIOT.Modules.Identity.Infrastructure.Services;

public sealed class EntraIdClaimsTransformation : IClaimsTransformation
{
    private readonly IServiceProvider _serviceProvider;

    public EntraIdClaimsTransformation(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.Identity is not ClaimsIdentity identity || !identity.IsAuthenticated)
        {
            return principal;
        }

        // Avoid re-transforming
        if (identity.HasClaim(c => c.Type == "identity_transformed"))
        {
            return principal;
        }

        // Extract Entra ID object id ('oid' or standard NameIdentifier)
        var externalId = identity.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value
            ?? identity.FindFirst("oid")?.Value
            ?? identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var email = identity.FindFirst(ClaimTypes.Email)?.Value
            ?? identity.FindFirst("preferred_username")?.Value
            ?? identity.FindFirst("upn")?.Value;

        if (string.IsNullOrWhiteSpace(externalId) && string.IsNullOrWhiteSpace(email))
        {
            return principal;
        }

        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

        // Find user by external ID or email
        var user = await dbContext.UserAccounts
            .Include(u => u.RoleAssignments)
                .ThenInclude(ra => ra.Role)
                    .ThenInclude(r => r.RolePermissions)
                        .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u =>
                (externalId != null && u.ExternalIdentityId == externalId) ||
                (email != null && u.Email.ToLower() == email.ToLower()));

        if (user is null)
        {
            // Auto-provision if user authenticated via Entra ID corporate tenant
            var tenantId = identity.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid")?.Value
                ?? identity.FindFirst("tid")?.Value;

            var name = identity.FindFirst("name")?.Value ?? email;
            var isInternal = !string.IsNullOrEmpty(tenantId); // Internal user if has organization tenant id

            user = new UserAccount
            {
                Email = email ?? $"{externalId}@ciot.local",
                DisplayName = name,
                ExternalIdentityId = externalId,
                AuthProvider = isInternal ? "EntraID" : "EntraExternalId",
                UserType = isInternal ? "Internal" : "External",
                Status = "Active",
                LastLoginAtUtc = DateTime.UtcNow
            };

            dbContext.UserAccounts.Add(user);
            await dbContext.SaveChangesAsync();
        }
        else
        {
            user.RecordLogin();
            if (string.IsNullOrEmpty(user.ExternalIdentityId) && !string.IsNullOrEmpty(externalId))
            {
                user.BindExternalIdentity(externalId, user.UserType == "Internal" ? "EntraID" : "EntraExternalId");
            }
            await dbContext.SaveChangesAsync();
        }

        // Attach enriched claims
        identity.AddClaim(new Claim("user_id", user.Id.ToString()));
        identity.AddClaim(new Claim("user_type", user.UserType));
        identity.AddClaim(new Claim("user_status", user.Status));

        foreach (var ra in user.RoleAssignments)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, ra.Role.Name));
            foreach (var rp in ra.Role.RolePermissions)
            {
                identity.AddClaim(new Claim("permission", rp.Permission.Code));
            }
        }

        identity.AddClaim(new Claim("identity_transformed", "true"));
        return principal;
    }
}
