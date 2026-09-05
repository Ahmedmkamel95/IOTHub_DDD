using CIOT.Modules.CustomerOutlet.Application.Commands;
using CIOT.Common.Behaviors;
using CIOT.Modules.CustomerOutlet.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CIOT.Modules.CustomerOutlet;

public static class CustomerOutletModuleExtensions
{
    public static IServiceCollection AddCustomerOutletModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ciot-db")
            ?? configuration.GetConnectionString("postgres")
            ?? "Host=localhost;Port=5432;Database=ciot-db;Username=postgres;Password=postgres";

        services.AddDbContext<CustomerOutletDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory_CustomerOutlet", CustomerOutletDbContext.Schema);
            });
            options.UseSnakeCaseNamingConvention();
        });

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CustomerOutletModuleExtensions).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(CreateCustomerCommand).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(CreateCustomerCommand).Assembly);

        services.AddScoped<CIOT.Common.Contracts.CustomerOutlet.ICustomerOutletApi, Endpoints.CustomerOutletEndpointClient>();

        return services;
    }
}

