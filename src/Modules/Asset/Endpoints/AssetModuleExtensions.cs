using CIOT.Modules.Asset.Application.Commands;
using CIOT.Common.Behaviors;
using CIOT.Modules.Asset.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CIOT.Modules.Asset;

public static class AssetModuleExtensions
{
    public static IServiceCollection AddAssetModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ciot-db")
            ?? configuration.GetConnectionString("postgres")
            ?? "Host=localhost;Port=5432;Database=ciot-db;Username=postgres;Password=postgres";

        services.AddDbContext<AssetDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory_Asset", AssetDbContext.Schema);
            });
            options.UseSnakeCaseNamingConvention();
        });

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(AssetModuleExtensions).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(RegisterAssetCommand).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(RegisterAssetCommand).Assembly);

        services.AddScoped<CIOT.Modules.Asset.Application.Contracts.ICustomerOutletValidator, CIOT.Modules.Asset.Endpoints.CustomerOutletValidatorAdapter>();

        return services;
    }
}

