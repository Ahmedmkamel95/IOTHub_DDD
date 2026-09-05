using CIOT.Common.Data;
using CIOT.Modules.Provisioning.Domain;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.Provisioning.Infrastructure;

public sealed class ProvisioningDbContext : BaseDbContext
{
    public const string Schema = "provisioning";

    public ProvisioningDbContext(DbContextOptions<ProvisioningDbContext> options) : base(options) { }

    public DbSet<DeviceManufacturer> DeviceManufacturers => Set<DeviceManufacturer>();
    public DbSet<AssetManufacturer> AssetManufacturers => Set<AssetManufacturer>();
    public DbSet<DeviceModel> DeviceModels => Set<DeviceModel>();
    public DbSet<ManufacturerDevice> ManufacturerDevices => Set<ManufacturerDevice>();
    public DbSet<DeviceAssetPairing> DeviceAssetPairings => Set<DeviceAssetPairing>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schema);

        modelBuilder.Entity<DeviceManufacturer>(b =>
        {
            b.ToTable("device_manufacturers");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.ManufacturerCode).IsUnique();
            b.Property(x => x.ManufacturerCode).HasMaxLength(50).IsRequired();
            b.Property(x => x.DisplayName).HasMaxLength(150).IsRequired();
        });

        modelBuilder.Entity<AssetManufacturer>(b =>
        {
            b.ToTable("asset_manufacturers");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.ManufacturerCode).IsUnique();
            b.Property(x => x.ManufacturerCode).HasMaxLength(50).IsRequired();
        });

        modelBuilder.Entity<DeviceModel>(b =>
        {
            b.ToTable("device_models");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.ModelCode).IsUnique();
            b.Property(x => x.ModelCode).HasMaxLength(50).IsRequired();

            b.HasOne(x => x.DeviceManufacturer)
                .WithMany(m => m.DeviceModels)
                .HasForeignKey(x => x.DeviceManufacturerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ManufacturerDevice>(b =>
        {
            b.ToTable("manufacturer_devices");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.SerialNumber).IsUnique();
            b.Property(x => x.SerialNumber).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<DeviceAssetPairing>(b =>
        {
            b.ToTable("device_asset_pairings");
            b.HasKey(x => x.Id);
            b.HasIndex(x => new { x.DeviceId, x.AssetId });
        });
    }
}
