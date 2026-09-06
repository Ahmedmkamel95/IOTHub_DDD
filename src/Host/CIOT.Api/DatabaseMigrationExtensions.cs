using CIOT.Modules.Admin.Infrastructure;
using CIOT.Modules.Asset.Infrastructure;
using CIOT.Modules.Audit.Infrastructure;
using CIOT.Modules.Catalog.Infrastructure;
using CIOT.Modules.CustomerOutlet.Infrastructure;
using CIOT.Modules.Devices.Infrastructure;
using CIOT.Modules.Identity.Infrastructure;
using CIOT.Modules.Integration.Infrastructure;
using CIOT.Modules.LocalAdapter.Infrastructure;
using CIOT.Modules.Mobile.Infrastructure;
using CIOT.Modules.Org.Infrastructure;
using CIOT.Modules.Provisioning.Infrastructure;
using CIOT.Modules.Report.Infrastructure;
using CIOT.Modules.Telemetry.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Api;

internal static class DatabaseMigrationExtensions
{
    public static async Task ApplyDatabaseMigrationsAsync(
        this WebApplication app,
        CancellationToken cancellationToken = default
    )
    {
        await using var scope = app.Services.CreateAsyncScope();

        var contexts = new DbContext[]
        {
            scope.ServiceProvider.GetRequiredService<AdminDbContext>(),
            scope.ServiceProvider.GetRequiredService<AssetDbContext>(),
            scope.ServiceProvider.GetRequiredService<AuditDbContext>(),
            scope.ServiceProvider.GetRequiredService<CatalogDbContext>(),
            scope.ServiceProvider.GetRequiredService<CustomerOutletDbContext>(),
            scope.ServiceProvider.GetRequiredService<DevicesDbContext>(),
            scope.ServiceProvider.GetRequiredService<IdentityDbContext>(),
            scope.ServiceProvider.GetRequiredService<IntegrationDbContext>(),
            scope.ServiceProvider.GetRequiredService<LocalAdapterDbContext>(),
            scope.ServiceProvider.GetRequiredService<MobileDbContext>(),
            scope.ServiceProvider.GetRequiredService<OrgDbContext>(),
            scope.ServiceProvider.GetRequiredService<ProvisioningDbContext>(),
            scope.ServiceProvider.GetRequiredService<ReportDbContext>(),
            scope.ServiceProvider.GetRequiredService<TelemetryDbContext>()
        };

        foreach (var context in contexts)
        {
            await context.Database.MigrateAsync(cancellationToken);
        }
    }
}
