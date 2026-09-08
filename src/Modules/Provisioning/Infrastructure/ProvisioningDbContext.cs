using CIOT.Common.Data;
using CIOT.Modules.Provisioning.Domain.Entities;
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
    public DbSet<ManufacturerAsset> ManufacturerAssets => Set<ManufacturerAsset>();
    public DbSet<AuthProviderConfig> AuthProviderConfigs => Set<AuthProviderConfig>();
    public DbSet<TelemetryDictionaryPackage> TelemetryDictionaryPackages => Set<TelemetryDictionaryPackage>();
    public DbSet<RecipeChangeHistory> RecipeChangeHistories => Set<RecipeChangeHistory>();
    public DbSet<ManufacturerDeviceImportItem> ManufacturerDeviceImportItems => Set<ManufacturerDeviceImportItem>();
    public DbSet<ManufacturerDeviceImportBatch> ManufacturerDeviceImportBatches => Set<ManufacturerDeviceImportBatch>();
    public DbSet<DeviceFirmwareUpdate> DeviceFirmwareUpdates => Set<DeviceFirmwareUpdate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProvisioningDbContext).Assembly);

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
                .WithMany()
                .HasForeignKey(x => x.DeviceManufacturerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ManufacturerDevice>(b =>
        {
            b.ToTable("manufacturer_devices");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.DeviceSerialNumber).IsUnique();
            b.Property(x => x.DeviceSerialNumber).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<DeviceAssetPairing>(b =>
        {
            b.ToTable("device_asset_pairings");
            b.HasKey(x => x.Id);
            b.HasIndex(x => new { x.ManufacturerDeviceId, x.ManufacturerAssetId });
        });
    }
}
