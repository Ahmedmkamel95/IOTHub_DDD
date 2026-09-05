using CIOT.Modules.Identity.Application.Commands;
using CIOT.Common.Behaviors;
using CIOT.Modules.Identity.Infrastructure;
using CIOT.Modules.Identity.Infrastructure.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;

namespace CIOT.Modules.Identity;

public static class IdentityModuleExtensions
{
    public static IServiceCollection AddIdentityModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ciot-db")
            ?? configuration.GetConnectionString("postgres")
            ?? "Host=localhost;Port=5432;Database=ciot-db;Username=postgres;Password=postgres";

        services.AddDbContext<IdentityDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory_Identity", IdentityDbContext.Schema);
            });
            options.UseSnakeCaseNamingConvention();
        });

        // Register MediatR handlers and FluentValidation validators from this assembly
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(IdentityModuleExtensions).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(AssignRoleCommand).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(AssignRoleCommand).Assembly);

        // SSO with Microsoft Entra ID (Azure AD) + Entra External ID
        var azureAdSection = configuration.GetSection("AzureAd");
        if (azureAdSection.Exists())
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(azureAdSection);
        }
        else
        {
            // Default JWT Bearer fallback for local/dev environments
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                });
        }

        services.AddAuthorization();
        services.AddTransient<IClaimsTransformation, EntraIdClaimsTransformation>();

        return services;
    }
}

