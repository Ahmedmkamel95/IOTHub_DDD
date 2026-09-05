using CIOT.Common.Data;
using CIOT.Modules.Telemetry.Domain;
using CmdScale.EntityFrameworkCore.TimescaleDB;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.Telemetry.Infrastructure;

public sealed class TelemetryDbContext : BaseDbContext
{
    public const string Schema = "telemetry";

    public TelemetryDbContext(DbContextOptions<TelemetryDbContext> options) : base(options) { }

    public DbSet<RawMessage> RawMessages => Set<RawMessage>();
    public DbSet<NormalizedMeasurement> NormalizedMeasurements => Set<NormalizedMeasurement>();
    public DbSet<NormalizedEvent> NormalizedEvents => Set<NormalizedEvent>();
    public DbSet<AssetCurrentState> AssetCurrentStates => Set<AssetCurrentState>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schema);

        modelBuilder.Entity<RawMessage>(b =>
        {
            b.ToTable("raw_messages");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.MessageId).IsUnique();
            b.Property(x => x.MessageId).HasMaxLength(128).IsRequired();
            b.Property(x => x.Status).HasMaxLength(50).IsRequired();
        });

        modelBuilder.Entity<NormalizedMeasurement>(b =>
        {
            b.ToTable("normalized_measurements");
            b.HasKey(x => x.Id);
            b.HasIndex(x => new { x.DeviceId, x.MetricKey, x.MeasuredAtUtc });
            b.HasIndex(x => new { x.AssetId, x.MetricKey, x.MeasuredAtUtc });
            b.Property(x => x.MetricKey).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<NormalizedEvent>(b =>
        {
            b.ToTable("normalized_events");
            b.HasKey(x => x.Id);
            b.HasIndex(x => new { x.DeviceId, x.EventType, x.EventOccurredAtUtc });
            b.Property(x => x.EventType).HasMaxLength(100).IsRequired();
            b.Property(x => x.Severity).HasMaxLength(50).IsRequired();
        });

        modelBuilder.Entity<AssetCurrentState>(b =>
        {
            b.ToTable("asset_current_states");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.AssetId).IsUnique();
            b.Property(x => x.MachineStatus).HasMaxLength(50);
        });
    }
}
