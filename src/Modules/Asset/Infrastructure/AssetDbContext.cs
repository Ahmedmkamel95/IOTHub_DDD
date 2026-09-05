using CIOT.Common.Data;
using CIOT.Modules.Asset.Domain;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.Asset.Infrastructure;

public sealed class AssetDbContext : BaseDbContext
{
    public const string Schema = "asset";

    public AssetDbContext(DbContextOptions<AssetDbContext> options) : base(options) { }

    public DbSet<Domain.Asset> Assets => Set<Domain.Asset>();
    public DbSet<AssetOutletAssignment> AssetOutletAssignments => Set<AssetOutletAssignment>();
    public DbSet<AssetWaterFilter> AssetWaterFilters => Set<AssetWaterFilter>();
    public DbSet<AssetIdentifier> AssetIdentifiers => Set<AssetIdentifier>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schema);

        modelBuilder.Entity<Domain.Asset>(b =>
        {
            b.ToTable("assets");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.SapEquipmentNumber).IsUnique();
            b.Property(x => x.SapEquipmentNumber).HasMaxLength(50).IsRequired();
            b.Property(x => x.CountryCode).HasMaxLength(10).IsRequired();
        });

        modelBuilder.Entity<AssetOutletAssignment>(b =>
        {
            b.ToTable("asset_outlet_assignments");
            b.HasKey(x => x.Id);
            b.HasIndex(x => new { x.AssetId, x.IsCurrent });

            b.HasOne(x => x.Asset)
                .WithMany(a => a.OutletAssignments)
                .HasForeignKey(x => x.AssetId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AssetWaterFilter>(b =>
        {
            b.ToTable("asset_water_filters");
            b.HasKey(x => x.Id);
            b.HasOne(x => x.Asset)
                .WithMany(a => a.WaterFilters)
                .HasForeignKey(x => x.AssetId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AssetIdentifier>(b =>
        {
            b.ToTable("asset_identifiers");
            b.HasKey(x => x.Id);
            b.HasIndex(x => new { x.IdentifierType, x.IdentifierValue });
            b.Property(x => x.IdentifierType).HasMaxLength(50).IsRequired();
            b.Property(x => x.IdentifierValue).HasMaxLength(100).IsRequired();

            b.HasOne(x => x.Asset)
                .WithMany(a => a.Identifiers)
                .HasForeignKey(x => x.AssetId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
