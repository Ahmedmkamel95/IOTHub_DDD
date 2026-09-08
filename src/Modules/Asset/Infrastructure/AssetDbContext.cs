using CIOT.Common.Data;
using CIOT.Modules.Asset.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using AssetEntity = CIOT.Modules.Asset.Domain.Entities.Asset;

namespace CIOT.Modules.Asset.Infrastructure;

public sealed class AssetDbContext : BaseDbContext
{
    public const string Schema = "asset";

    public AssetDbContext(DbContextOptions<AssetDbContext> options) : base(options) { }

    public DbSet<AssetEntity> Assets => Set<AssetEntity>();
    public DbSet<AssetOutletAssignment> AssetOutletAssignments => Set<AssetOutletAssignment>();
    public DbSet<AssetWaterFilter> AssetWaterFilters => Set<AssetWaterFilter>();
    public DbSet<AssetIdentifier> AssetIdentifiers => Set<AssetIdentifier>();
    public DbSet<AssetExternalIdentity> AssetExternalIdentities => Set<AssetExternalIdentity>();
    public DbSet<AssetStatusHistory> AssetStatusHistories => Set<AssetStatusHistory>();
    public DbSet<AssetTarget> AssetTargets => Set<AssetTarget>();
    public DbSet<WaterFilterReset> WaterFilterResets => Set<WaterFilterReset>();
    public DbSet<AssetType> AssetTypes => Set<AssetType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssetDbContext).Assembly);

        modelBuilder.Entity<AssetEntity>(b =>
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
            b.HasIndex(x => new { x.AssetId, x.RemovedAtUtc });

            b.HasOne<AssetEntity>()
                .WithMany()
                .HasForeignKey(x => x.AssetId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AssetWaterFilter>(b =>
        {
            b.ToTable("asset_water_filters");
            b.HasKey(x => x.Id);
            b.HasOne<AssetEntity>()
                .WithMany()
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

            b.HasOne<AssetEntity>()
                .WithMany()
                .HasForeignKey(x => x.AssetId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
