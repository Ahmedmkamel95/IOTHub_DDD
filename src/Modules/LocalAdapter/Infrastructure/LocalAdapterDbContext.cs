using CIOT.Common.Data;
using CIOT.Modules.LocalAdapter.Domain;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.LocalAdapter.Infrastructure;

public sealed class LocalAdapterDbContext : BaseDbContext
{
    public const string Schema = "local_adapter";

    public LocalAdapterDbContext(DbContextOptions<LocalAdapterDbContext> options) : base(options) { }

    public DbSet<DeviceProjectionEffect> DeviceProjectionEffects => Set<DeviceProjectionEffect>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schema);

        modelBuilder.Entity<DeviceProjectionEffect>(b =>
        {
            b.ToTable("device_projection_effects");
            b.HasKey(x => x.Id);
            b.HasIndex(x => new { x.DeviceId, x.Status });
        });
    }
}
