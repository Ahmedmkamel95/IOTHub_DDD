using CIOT.Modules.Catalog.Application;
using CIOT.Common.Behaviors;
using CIOT.Modules.Catalog.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CIOT.Modules.Catalog;

public static class CatalogModuleExtensions
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ciot-db")
            ?? configuration.GetConnectionString("postgres")
            ?? "Host=localhost;Port=5432;Database=ciot-db;Username=postgres;Password=postgres";

        services.AddDbContext<CatalogDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory_Catalog", CatalogDbContext.Schema);
            });
            options.UseSnakeCaseNamingConvention();
        });

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CatalogModuleExtensions).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(CreateMaterialCommand).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        return services;
    }
}

