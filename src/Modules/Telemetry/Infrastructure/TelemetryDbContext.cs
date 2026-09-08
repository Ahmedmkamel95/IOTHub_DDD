using CIOT.Common.Data;
using CIOT.Modules.Telemetry.Domain.Entities;
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
    public DbSet<WaterConsumptionMeasurement> WaterConsumptionMeasurements => Set<WaterConsumptionMeasurement>();
    public DbSet<VendEvent> VendEvents => Set<VendEvent>();
    public DbSet<QualityEvent> QualityEvents => Set<QualityEvent>();
    public DbSet<PersistenceDeadLetter> PersistenceDeadLetters => Set<PersistenceDeadLetter>();
    public DbSet<NormalizedMeasurementValueRollupDaily> NormalizedMeasurementValueRollupsDaily => Set<NormalizedMeasurementValueRollupDaily>();
    public DbSet<NormalizedMeasurementValueRollup15Minute> NormalizedMeasurementValueRollups15Minute => Set<NormalizedMeasurementValueRollup15Minute>();
    public DbSet<MachineStatusReport> MachineStatusReports => Set<MachineStatusReport>();
    public DbSet<LocationResolutionOutbox> LocationResolutionOutboxes => Set<LocationResolutionOutbox>();
    public DbSet<LocationReport> LocationReports => Set<LocationReport>();
    public DbSet<LocalNormalizedMessageQueue> LocalNormalizedMessageQueues => Set<LocalNormalizedMessageQueue>();
    public DbSet<LocalLocationResolutionQueue> LocalLocationResolutionQueues => Set<LocalLocationResolutionQueue>();
    public DbSet<IngredientConsumptionMeasurement> IngredientConsumptionMeasurements => Set<IngredientConsumptionMeasurement>();
    public DbSet<EvadtsCounterMeasurement> EvadtsCounterMeasurements => Set<EvadtsCounterMeasurement>();
    public DbSet<ErrorEvent> ErrorEvents => Set<ErrorEvent>();
    public DbSet<EnergyConsumptionMeasurement> EnergyConsumptionMeasurements => Set<EnergyConsumptionMeasurement>();
    public DbSet<ConnectivityReport> ConnectivityReports => Set<ConnectivityReport>();
    public DbSet<CoffeeConsumptionMeasurement> CoffeeConsumptionMeasurements => Set<CoffeeConsumptionMeasurement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TelemetryDbContext).Assembly);

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
            b.HasIndex(x => new { x.DeviceId, x.MetricKey, x.ObservedAtUtc });
            b.HasIndex(x => new { x.AssetId, x.MetricKey, x.ObservedAtUtc });
            b.Property(x => x.MetricKey).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<NormalizedEvent>(b =>
        {
            b.ToTable("normalized_events");
            b.HasKey(x => x.Id);
            b.HasIndex(x => new { x.DeviceId, x.EventType, x.EventAtUtc });
            b.Property(x => x.EventType).HasMaxLength(100).IsRequired();
            b.Property(x => x.Severity).HasMaxLength(50).IsRequired();
        });

        modelBuilder.Entity<AssetCurrentState>(b =>
        {
            b.ToTable("asset_current_states");
            b.HasKey(x => x.AssetId);
            b.Property(x => x.MachineStatus).HasMaxLength(50);
        });
    }
}
