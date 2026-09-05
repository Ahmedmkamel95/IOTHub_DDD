using CIOT.Modules.Integration.Application;
using CIOT.Common.Behaviors;
using CIOT.Modules.Integration.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CIOT.Modules.Integration;

public static class IntegrationModuleExtensions
{
    public static IServiceCollection AddIntegrationModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ciot-db")
            ?? configuration.GetConnectionString("postgres")
            ?? "Host=localhost;Port=5432;Database=ciot-db;Username=postgres;Password=postgres";

        services.AddDbContext<IntegrationDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory_Integration", IntegrationDbContext.Schema);
            });
            options.UseSnakeCaseNamingConvention();
        });

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(IntegrationModuleExtensions).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(CreatePartnerSourceCommand).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        return services;
    }
}

