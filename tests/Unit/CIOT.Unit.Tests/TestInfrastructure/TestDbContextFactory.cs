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

namespace CIOT.Unit.Tests.TestInfrastructure;

internal static class TestDbContextFactory
{
    internal static OrgDbContext CreateOrgContext()
    {
        var options = new DbContextOptionsBuilder<OrgDbContext>()
            .UseInMemoryDatabase($"ciot-unit-{Guid.NewGuid():N}")
            .Options;

        return new OrgDbContext(options);
    }

    internal static DevicesDbContext CreateDevicesContext()
    {
        var options = new DbContextOptionsBuilder<DevicesDbContext>()
            .UseInMemoryDatabase($"ciot-unit-devices-{Guid.NewGuid():N}")
            .Options;

        return new DevicesDbContext(options);
    }

    internal static AssetDbContext CreateAssetContext()
    {
        var options = new DbContextOptionsBuilder<AssetDbContext>()
            .UseInMemoryDatabase($"ciot-unit-assets-{Guid.NewGuid():N}")
            .Options;

        return new AssetDbContext(options);
    }

    internal static CustomerOutletDbContext CreateCustomerOutletContext()
    {
        var options = new DbContextOptionsBuilder<CustomerOutletDbContext>()
            .UseInMemoryDatabase($"ciot-unit-customer-outlet-{Guid.NewGuid():N}")
            .Options;

        return new CustomerOutletDbContext(options);
    }

    internal static IdentityDbContext CreateIdentityContext()
    {
        var options = new DbContextOptionsBuilder<IdentityDbContext>()
            .UseInMemoryDatabase($"ciot-unit-identity-{Guid.NewGuid():N}")
            .Options;

        return new IdentityDbContext(options);
    }

    internal static ProvisioningDbContext CreateProvisioningContext()
    {
        var options = new DbContextOptionsBuilder<ProvisioningDbContext>()
            .UseInMemoryDatabase($"ciot-unit-provisioning-{Guid.NewGuid():N}")
            .Options;

        return new ProvisioningDbContext(options);
    }

    internal static TelemetryDbContext CreateTelemetryContext()
    {
        var options = new DbContextOptionsBuilder<TelemetryDbContext>()
            .UseInMemoryDatabase($"ciot-unit-telemetry-{Guid.NewGuid():N}")
            .Options;

        return new TelemetryDbContext(options);
    }

    internal static CatalogDbContext CreateCatalogContext()
    {
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseInMemoryDatabase($"ciot-unit-catalog-{Guid.NewGuid():N}")
            .Options;

        return new CatalogDbContext(options);
    }

    internal static AdminDbContext CreateAdminContext() => Create<AdminDbContext>();
    internal static AuditDbContext CreateAuditContext() => Create<AuditDbContext>();
    internal static IntegrationDbContext CreateIntegrationContext() => Create<IntegrationDbContext>();
    internal static LocalAdapterDbContext CreateLocalAdapterContext() => Create<LocalAdapterDbContext>();
    internal static MobileDbContext CreateMobileContext() => Create<MobileDbContext>();
    internal static ReportDbContext CreateReportContext() => Create<ReportDbContext>();

    private static TContext Create<TContext>()
        where TContext : DbContext
    {
        var options = new DbContextOptionsBuilder<TContext>()
            .UseInMemoryDatabase($"ciot-unit-{typeof(TContext).Name}-{Guid.NewGuid():N}")
            .Options;

        return (TContext)Activator.CreateInstance(typeof(TContext), options)!;
    }
}
