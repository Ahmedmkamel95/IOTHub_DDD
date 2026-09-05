using CIOT.Common.Data;
using CIOT.Modules.CustomerOutlet.Domain;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.CustomerOutlet.Infrastructure;

public sealed class CustomerOutletDbContext : BaseDbContext
{
    public const string Schema = "customer_outlet";

    public CustomerOutletDbContext(DbContextOptions<CustomerOutletDbContext> options) : base(options) { }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Outlet> Outlets => Set<Outlet>();
    public DbSet<OutletNote> OutletNotes => Set<OutletNote>();
    public DbSet<CustomerCluster> CustomerClusters => Set<CustomerCluster>();
    public DbSet<CustomerRelationship> CustomerRelationships => Set<CustomerRelationship>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schema);

        modelBuilder.Entity<Customer>(b =>
        {
            b.ToTable("customers");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.CustomerCode).IsUnique();
            b.Property(x => x.CustomerCode).HasMaxLength(50).IsRequired();
            b.Property(x => x.CountryCode).HasMaxLength(10).IsRequired();

            b.HasOne(x => x.CustomerCluster)
                .WithMany()
                .HasForeignKey(x => x.CustomerClusterId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Outlet>(b =>
        {
            b.ToTable("outlets");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.OutletCode).IsUnique();
            b.Property(x => x.OutletCode).HasMaxLength(50).IsRequired();
            b.Property(x => x.CountryCode).HasMaxLength(10).IsRequired();

            b.HasOne(x => x.Customer)
                .WithMany(c => c.Outlets)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<OutletNote>(b =>
        {
            b.ToTable("outlet_notes");
            b.HasKey(x => x.Id);
            b.Property(x => x.NoteBody).HasMaxLength(2000).IsRequired();

            b.HasOne(x => x.Outlet)
                .WithMany(o => o.Notes)
                .HasForeignKey(x => x.OutletId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CustomerCluster>(b =>
        {
            b.ToTable("customer_clusters");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.ClusterCode).IsUnique();
            b.Property(x => x.ClusterCode).HasMaxLength(50).IsRequired();
        });

        modelBuilder.Entity<CustomerRelationship>(b =>
        {
            b.ToTable("customer_relationships");
            b.HasKey(x => x.Id);
        });
    }
}
