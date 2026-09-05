using CIOT.Common.Data;
using CIOT.Modules.Devices.Domain;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.Devices.Infrastructure;

public sealed class DevicesDbContext : BaseDbContext
{
    public const string Schema = "devices";

    public DevicesDbContext(DbContextOptions<DevicesDbContext> options) : base(options) { }

    public DbSet<Device> Devices => Set<Device>();
    public DbSet<DeviceAssignment> DeviceAssignments => Set<DeviceAssignment>();
    public DbSet<DeviceCommand> DeviceCommands => Set<DeviceCommand>();
    public DbSet<DeviceCertificate> DeviceCertificates => Set<DeviceCertificate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schema);

        modelBuilder.Entity<Device>(b =>
        {
            b.ToTable("devices");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.IotHubDeviceId).IsUnique();
            b.Property(x => x.IotHubDeviceId).HasMaxLength(128).IsRequired();
            b.Property(x => x.LifecycleStatus).HasMaxLength(50).IsRequired();
            b.Property(x => x.CountryCode).HasMaxLength(10);
            b.Property(x => x.DeviceSerialNumber).HasMaxLength(100);
        });

        modelBuilder.Entity<DeviceAssignment>(b =>
        {
            b.ToTable("device_assignments");
            b.HasKey(x => x.Id);
            b.HasIndex(x => new { x.DeviceId, x.AssetId });

            b.HasOne(x => x.Device)
                .WithMany(d => d.Assignments)
                .HasForeignKey(x => x.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DeviceCommand>(b =>
        {
            b.ToTable("device_commands");
            b.HasKey(x => x.Id);
            b.HasIndex(x => new { x.DeviceId, x.Status });
            b.Property(x => x.CommandType).HasMaxLength(100).IsRequired();
            b.Property(x => x.Status).HasMaxLength(50).IsRequired();

            b.HasOne(x => x.Device)
                .WithMany(d => d.Commands)
                .HasForeignKey(x => x.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DeviceCertificate>(b =>
        {
            b.ToTable("device_certificates");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.Thumbprint).IsUnique();
            b.Property(x => x.Thumbprint).HasMaxLength(128).IsRequired();

            b.HasOne(x => x.Device)
                .WithMany()
                .HasForeignKey(x => x.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
