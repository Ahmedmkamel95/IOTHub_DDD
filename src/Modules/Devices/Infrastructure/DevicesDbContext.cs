using CIOT.Common.Data;
using CIOT.Modules.Devices.Domain.Entities;
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
    public DbSet<DeviceIdentifier> DeviceIdentifiers => Set<DeviceIdentifier>();
    public DbSet<DeviceFirmwareAssignment> DeviceFirmwareAssignments => Set<DeviceFirmwareAssignment>();
    public DbSet<DeviceFirmwareStatusEvent> DeviceFirmwareStatusEvents => Set<DeviceFirmwareStatusEvent>();
    public DbSet<DeviceBusinessAssignmentSchedule> DeviceBusinessAssignmentSchedules => Set<DeviceBusinessAssignmentSchedule>();
    public DbSet<DeviceBulkAction> DeviceBulkActions => Set<DeviceBulkAction>();
    public DbSet<DeviceBulkActionItem> DeviceBulkActionItems => Set<DeviceBulkActionItem>();
    public DbSet<DeviceFirmwareState> DeviceFirmwareStates => Set<DeviceFirmwareState>();
    public DbSet<DeviceLifecycleState> DeviceLifecycleStates => Set<DeviceLifecycleState>();
    public DbSet<DeviceLifecycleOutbox> DeviceLifecycleOutboxes => Set<DeviceLifecycleOutbox>();
    public DbSet<FirmwareArtifact> FirmwareArtifacts => Set<FirmwareArtifact>();
    public DbSet<FirmwareArtifactDeviceModel> FirmwareArtifactDeviceModels => Set<FirmwareArtifactDeviceModel>();
    public DbSet<FirmwareArtifactEquipmentModel> FirmwareArtifactEquipmentModels => Set<FirmwareArtifactEquipmentModel>();
    public DbSet<FotaDownloadGrant> FotaDownloadGrants => Set<FotaDownloadGrant>();
    public DbSet<FotaUpdateSession> FotaUpdateSessions => Set<FotaUpdateSession>();
    public DbSet<FirmwareAssignmentHandoffOutbox> FirmwareAssignmentHandoffOutboxes => Set<FirmwareAssignmentHandoffOutbox>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DevicesDbContext).Assembly);

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
            b.HasIndex(x => new { x.DeviceId, x.CommandStatus });
            b.Property(x => x.CommandType).HasMaxLength(100).IsRequired();
            b.Property(x => x.CommandStatus).HasMaxLength(50).IsRequired();

            b.HasOne(x => x.Device)
                .WithMany()
                .HasForeignKey(x => x.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DeviceCertificate>(b =>
        {
            b.ToTable("device_certificates");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.Thumbprint).IsUnique();
            b.Property(x => x.Thumbprint).HasMaxLength(128).IsRequired();

            b.HasOne<Device>()
                .WithMany()
                .HasForeignKey(x => x.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
