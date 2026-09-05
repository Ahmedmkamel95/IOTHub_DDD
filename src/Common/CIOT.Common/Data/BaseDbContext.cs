using CIOT.Common.Domain;
using EFCore.NamingConventions;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Common.Data;

public abstract class BaseDbContext : DbContext
{
    protected BaseDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Ignore<IDomainEvent>();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditProperties();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        UpdateAuditProperties();
        return base.SaveChanges();
    }

    private void UpdateAuditProperties()
    {
        var entries = ChangeTracker.Entries();
        var now = DateTime.UtcNow;

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                if (entry.Entity is ICreatedAuditableEntity created)
                {
                    if (created.CreatedAtUtc == default)
                    {
                        created.CreatedAtUtc = now;
                    }
                }
            }
            else if (entry.State == EntityState.Modified)
            {
                if (entry.Entity is IUpdatedAuditableEntity updated)
                {
                    updated.ModifiedAtUtc = now;
                }
            }
        }
    }
}
