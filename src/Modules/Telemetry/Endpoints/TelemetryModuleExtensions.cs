using CIOT.Modules.Telemetry.Application.Commands;
using CIOT.Common.Behaviors;
using CIOT.Modules.Telemetry.Infrastructure;
using CmdScale.EntityFrameworkCore.TimescaleDB;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CIOT.Modules.Telemetry;

public static class TelemetryModuleExtensions
{
    public static IServiceCollection AddTelemetryModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ciot-db")
            ?? configuration.GetConnectionString("postgres")
            ?? "Host=localhost;Port=5432;Database=ciot-db;Username=postgres;Password=postgres";

        services.AddDbContext<TelemetryDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.UseNetTopologySuite();
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory_Telemetry", TelemetryDbContext.Schema);
            });
            options.UseTimescaleDb();
            options.UseSnakeCaseNamingConvention();
        });

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(TelemetryModuleExtensions).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(IngestTelemetryCommand).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(IngestTelemetryCommand).Assembly);

        return services;
    }
}

