using CIOT.Modules.Admin;
using CIOT.Modules.Admin.Endpoints;
using CIOT.Modules.Asset;
using CIOT.Modules.Asset.Endpoints;
using CIOT.Modules.Audit;
using CIOT.Modules.Audit.Endpoints;
using CIOT.Modules.Catalog;
using CIOT.Modules.Catalog.Endpoints;
using CIOT.Modules.CustomerOutlet;
using CIOT.Modules.CustomerOutlet.Endpoints;
using CIOT.Modules.Devices;
using CIOT.Modules.Devices.Endpoints;
using CIOT.Modules.Identity;
using CIOT.Modules.Identity.Endpoints;
using CIOT.Modules.Integration;
using CIOT.Modules.Integration.Endpoints;
using CIOT.Modules.LocalAdapter;
using CIOT.Modules.LocalAdapter.Endpoints;
using CIOT.Modules.Mobile;
using CIOT.Modules.Mobile.Endpoints;
using CIOT.Modules.Org;
using CIOT.Modules.Org.Endpoints;
using CIOT.Modules.Provisioning;
using CIOT.Modules.Provisioning.Endpoints;
using CIOT.Modules.Report;
using CIOT.Modules.Report.Endpoints;
using CIOT.Modules.Telemetry;
using CIOT.Modules.Telemetry.Endpoints;
using CIOT.ServiceDefaults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Aspire Service Defaults (OpenTelemetry, Health checks, Resiliency)
builder.AddServiceDefaults();

// 2. OpenAPI & API Documentation
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// 3. Register All 14 Bounded Context Modules (DDD Light)
builder.Services.AddIdentityModule(builder.Configuration);
builder.Services.AddOrgModule(builder.Configuration);
builder.Services.AddCustomerOutletModule(builder.Configuration);
builder.Services.AddAssetModule(builder.Configuration);
builder.Services.AddDevicesModule(builder.Configuration);
builder.Services.AddTelemetryModule(builder.Configuration);
builder.Services.AddProvisioningModule(builder.Configuration);
builder.Services.AddCatalogModule(builder.Configuration);
builder.Services.AddAdminModule(builder.Configuration);
builder.Services.AddMobileModule(builder.Configuration);
builder.Services.AddIntegrationModule(builder.Configuration);
builder.Services.AddAuditModule(builder.Configuration);
builder.Services.AddReportModule(builder.Configuration);
builder.Services.AddLocalAdapterModule(builder.Configuration);

var app = builder.Build();

// Apply versioned migrations when explicitly requested; retain direct creation for local bootstrap.
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var dbContextTypes = new[]
    {
        typeof(CIOT.Modules.Identity.Infrastructure.IdentityDbContext),
        typeof(CIOT.Modules.Org.Infrastructure.OrgDbContext),
        typeof(CIOT.Modules.CustomerOutlet.Infrastructure.CustomerOutletDbContext),
        typeof(CIOT.Modules.Asset.Infrastructure.AssetDbContext),
        typeof(CIOT.Modules.Devices.Infrastructure.DevicesDbContext),
        typeof(CIOT.Modules.Telemetry.Infrastructure.TelemetryDbContext),
        typeof(CIOT.Modules.Provisioning.Infrastructure.ProvisioningDbContext),
        typeof(CIOT.Modules.Catalog.Infrastructure.CatalogDbContext),
        typeof(CIOT.Modules.Admin.Infrastructure.AdminDbContext),
        typeof(CIOT.Modules.Mobile.Infrastructure.MobileDbContext),
        typeof(CIOT.Modules.Integration.Infrastructure.IntegrationDbContext),
        typeof(CIOT.Modules.Audit.Infrastructure.AuditDbContext),
        typeof(CIOT.Modules.Report.Infrastructure.ReportDbContext),
        typeof(CIOT.Modules.LocalAdapter.Infrastructure.LocalAdapterDbContext)
    };

    foreach (var type in dbContextTypes)
    {
        if (scope.ServiceProvider.GetRequiredService(type) is DbContext ctx)
        {
            try
            {
                if (args.Contains("--migrate"))
                {
                    await ctx.Database.MigrateAsync();
                    logger.LogInformation("Database migrated for {Context}", type.Name);
                }
                else
                {
                    var creator = ctx.Database.GetService<IRelationalDatabaseCreator>();
                    await creator.CreateTablesAsync();
                    logger.LogInformation("Database tables created for {Context}", type.Name);
                }
            }
            catch (Exception ex)
            {
                if (args.Contains("--migrate"))
                {
                    logger.LogError(ex, "Migration failed for {Context}", type.Name);
                    throw;
                }

                logger.LogDebug(ex, "Tables already exist or skipped for {Context}", type.Name);
            }
        }
    }
}

if (args.Contains("--apply-missing-tables"))
{
    using var scope = app.Services.CreateScope();
    var ctx = scope.ServiceProvider.GetRequiredService<CIOT.Modules.Asset.Infrastructure.AssetDbContext>();
    var scriptsDir = Path.GetFullPath(Path.Combine(app.Environment.ContentRootPath, "..", "..", "..", "scripts"));
    var missingTablesPath = Path.Combine(scriptsDir, "add_missing_monolith_tables.sql");

    if (!File.Exists(missingTablesPath))
    {
        throw new FileNotFoundException("Missing-table migration script was not found.", missingTablesPath);
    }

    var sql = await File.ReadAllTextAsync(missingTablesPath);
    await ctx.Database.ExecuteSqlRawAsync(sql);
    Console.WriteLine($"Missing monolithic tables applied from {missingTablesPath}.");
    return;
}

if (args.Contains("--migrate"))
{
    Console.WriteLine("Database schema migration complete. Exiting.");
    return;
}

if (args.Contains("--seed"))
{
    Console.WriteLine("Seeding database test data across all Bounded Contexts...");
    using var scope = app.Services.CreateScope();
    var ctx = scope.ServiceProvider.GetRequiredService<CIOT.Modules.Asset.Infrastructure.AssetDbContext>();
    var scriptsDir = Path.GetFullPath(Path.Combine(app.Environment.ContentRootPath, "..", "..", "..", "scripts"));
    var seedPath = Path.Combine(scriptsDir, "seed_all_modules.sql");
    if (File.Exists(seedPath))
    {
        var sql = await File.ReadAllTextAsync(seedPath);
        await ctx.Database.ExecuteSqlRawAsync(sql);
        Console.WriteLine($"Database test data seeded successfully from {seedPath}!");
    }
    else
    {
        Console.WriteLine($"Seed file not found at {seedPath}");
    }
    return;
}

// 4. Middleware Pipeline
app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    // Generate OpenAPI Document at /openapi/v1.json
    app.MapOpenApi();

    // Classic Swagger UI at /swagger
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/openapi/v1.json", "CIOT.ModularHub API v1");
        c.RoutePrefix = "swagger";
        c.DocumentTitle = "CIOT ModularHub - Swagger UI";
    });

    // Modern Scalar API Reference at /scalar/v1
    app.MapScalarApiReference();
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

// 5. Map Minimal API Endpoints for All 14 Bounded Contexts
app.MapIdentityEndpoints();
app.MapOrgEndpoints();
app.MapCustomerOutletEndpoints();
app.MapAssetEndpoints();
app.MapDevicesEndpoints();
app.MapTelemetryEndpoints();
app.MapProvisioningEndpoints();
app.MapCatalogEndpoints();
app.MapAdminEndpoints();
app.MapMobileEndpoints();
app.MapIntegrationEndpoints();
app.MapAuditEndpoints();
app.MapReportEndpoints();
app.MapLocalAdapterEndpoints();

// Root landing endpoint
app.MapGet("/", () => Results.Ok(new
{
    Application = "CIOT_ModularHub",
    Architecture = "Modular Monolith (DDD Light, CQRS with MediatR, Minimal APIs)",
    ModulesCount = 14,
    SwaggerUI = "/swagger",
    ScalarUI = "/scalar/v1",
    OpenApiJson = "/openapi/v1.json",
    Boundaries = new[]
    {
        "Identity", "Org", "CustomerOutlet", "Asset", "Devices",
        "Provisioning", "Telemetry", "Catalog", "Admin", "Mobile",
        "Integration", "Audit", "Report", "LocalAdapter"
    },
    Status = "Healthy"
}))
.WithTags("System")
.ExcludeFromDescription();

await app.RunAsync();
