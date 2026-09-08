using CIOT.Common.Data;
using CIOT.Modules.Audit.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.Audit.Infrastructure;

public sealed class AuditDbContext : BaseDbContext
{
    public const string Schema = "audit";

    public AuditDbContext(DbContextOptions<AuditDbContext> options) : base(options) { }

    public DbSet<AuditEvent> AuditEvents => Set<AuditEvent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuditDbContext).Assembly);

        modelBuilder.Entity<AuditEvent>(b =>
        {
            b.ToTable("audit_events");
            b.HasKey(x => x.Id);
            b.HasIndex(x => new { x.EntityType, x.EntityId });
            b.HasIndex(x => x.EventAtUtc);
        });
    }
}
