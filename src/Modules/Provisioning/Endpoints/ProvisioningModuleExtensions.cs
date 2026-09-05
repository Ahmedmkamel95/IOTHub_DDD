using CIOT.Modules.Provisioning.Application.Commands;
using CIOT.Common.Behaviors;
using CIOT.Modules.Provisioning.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CIOT.Modules.Provisioning;

public static class ProvisioningModuleExtensions
{
    public static IServiceCollection AddProvisioningModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ciot-db")
            ?? configuration.GetConnectionString("postgres")
            ?? "Host=localhost;Port=5432;Database=ciot-db;Username=postgres;Password=postgres";

        services.AddDbContext<ProvisioningDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory_Provisioning", ProvisioningDbContext.Schema);
            });
            options.UseSnakeCaseNamingConvention();
        });

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ProvisioningModuleExtensions).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(CreateDeviceManufacturerCommand).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(CreateDeviceManufacturerCommand).Assembly);

        return services;
    }
}

