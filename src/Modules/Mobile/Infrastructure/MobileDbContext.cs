using CIOT.Common.Data;
using CIOT.Modules.Mobile.Domain;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.Mobile.Infrastructure;

public sealed class MobileDbContext : BaseDbContext
{
    public const string Schema = "mobile";

    public MobileDbContext(DbContextOptions<MobileDbContext> options) : base(options) { }

    public DbSet<OfflineBatch> OfflineBatches => Set<OfflineBatch>();
    public DbSet<OfflineActionResult> OfflineActionResults => Set<OfflineActionResult>();
    public DbSet<DeviceReplacement> DeviceReplacements => Set<DeviceReplacement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schema);

        modelBuilder.Entity<OfflineBatch>(b =>
        {
            b.ToTable("offline_batches");
            b.HasKey(x => x.Id);
        });

        modelBuilder.Entity<OfflineActionResult>(b =>
        {
            b.ToTable("offline_action_results");
            b.HasKey(x => x.Id);
            b.HasIndex(x => new { x.OfflineBatchId, x.ClientActionId });

            b.HasOne<OfflineBatch>()
                .WithMany(b => b.Results)
                .HasForeignKey(x => x.OfflineBatchId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DeviceReplacement>(b =>
        {
            b.ToTable("device_replacements");
            b.HasKey(x => x.Id);
            b.HasIndex(x => new { x.AssetId, x.ReplacedAtUtc });
        });
    }
}
