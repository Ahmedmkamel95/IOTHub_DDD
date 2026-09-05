using CIOT.Modules.Report.Application;
using CIOT.Common.Behaviors;
using CIOT.Modules.Report.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CIOT.Modules.Report;

public static class ReportModuleExtensions
{
    public static IServiceCollection AddReportModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ciot-db")
            ?? configuration.GetConnectionString("postgres")
            ?? "Host=localhost;Port=5432;Database=ciot-db;Username=postgres;Password=postgres";

        services.AddDbContext<ReportDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory_Report", ReportDbContext.Schema);
            });
            options.UseSnakeCaseNamingConvention();
        });

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ReportModuleExtensions).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(CreateReportDefinitionCommand).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        return services;
    }
}

