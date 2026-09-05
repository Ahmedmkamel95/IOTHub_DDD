using CIOT.Modules.Admin.Application;
using CIOT.Common.Behaviors;
using CIOT.Modules.Admin.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CIOT.Modules.Admin;

public static class AdminModuleExtensions
{
    public static IServiceCollection AddAdminModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ciot-db")
            ?? configuration.GetConnectionString("postgres")
            ?? "Host=localhost;Port=5432;Database=ciot-db;Username=postgres;Password=postgres";

        services.AddDbContext<AdminDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory_Admin", AdminDbContext.Schema);
            });
            options.UseSnakeCaseNamingConvention();
        });

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(AdminModuleExtensions).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(CreateEquipmentModelCommand).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        return services;
    }
}

