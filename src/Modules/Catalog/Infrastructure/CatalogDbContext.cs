using CIOT.Common.Data;
using CIOT.Modules.Catalog.Domain;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.Catalog.Infrastructure;

public sealed class CatalogDbContext : BaseDbContext
{
    public const string Schema = "catalog";

    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options) { }

    public DbSet<Material> Materials => Set<Material>();
    public DbSet<SkuMapping> SkuMappings => Set<SkuMapping>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schema);

        modelBuilder.Entity<Material>(b =>
        {
            b.ToTable("materials");
            b.HasKey(x => x.Id);
            b.HasIndex(x => new { x.MaterialCode, x.CountryCode }).IsUnique();
            b.Property(x => x.MaterialCode).HasMaxLength(50).IsRequired();
            b.Property(x => x.CountryCode).HasMaxLength(10).IsRequired();
        });

        modelBuilder.Entity<SkuMapping>(b =>
        {
            b.ToTable("sku_mappings");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.SkuCode).IsUnique();
        });
    }
}
