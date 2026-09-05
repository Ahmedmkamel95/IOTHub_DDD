using CIOT.Common.Data;
using CIOT.Modules.Admin.Domain;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.Admin.Infrastructure;

public sealed class AdminDbContext : BaseDbContext
{
    public const string Schema = "admin";

    public AdminDbContext(DbContextOptions<AdminDbContext> options) : base(options) { }

    public DbSet<EquipmentModel> EquipmentModels => Set<EquipmentModel>();
    public DbSet<OperationalStatusPolicy> OperationalStatusPolicies => Set<OperationalStatusPolicy>();
    public DbSet<ErrorMapping> ErrorMappings => Set<ErrorMapping>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schema);

        modelBuilder.Entity<EquipmentModel>(b =>
        {
            b.ToTable("equipment_models");
            b.HasKey(x => x.Id);
            b.HasIndex(x => new { x.Manufacturer, x.Model }).IsUnique();
        });

        modelBuilder.Entity<OperationalStatusPolicy>(b =>
        {
            b.ToTable("operational_status_policies");
            b.HasKey(x => x.Id);
        });

        modelBuilder.Entity<ErrorMapping>(b =>
        {
            b.ToTable("error_mappings");
            b.HasKey(x => x.Id);
            b.HasIndex(x => new { x.Manufacturer, x.RawErrorCode });
        });
    }
}
