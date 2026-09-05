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
