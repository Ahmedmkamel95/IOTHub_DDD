using CIOT.Common.Data;
using CIOT.Modules.Org.Domain;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.Org.Infrastructure;

public sealed class OrgDbContext : BaseDbContext
{
    public const string Schema = "org";

    public OrgDbContext(DbContextOptions<OrgDbContext> options) : base(options) { }

    public DbSet<Country> Countries => Set<Country>();
    public DbSet<BusinessUnit> BusinessUnits => Set<BusinessUnit>();
    public DbSet<SalesTerritory> SalesTerritories => Set<SalesTerritory>();
    public DbSet<SalesOrganization> SalesOrganizations => Set<SalesOrganization>();
    public DbSet<BusinessUnitCountry> BusinessUnitCountries => Set<BusinessUnitCountry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schema);

        modelBuilder.Entity<Country>(b =>
        {
            b.ToTable("countries");
            b.HasKey(x => x.CountryCode);
            b.Property(x => x.CountryCode).HasMaxLength(10).IsRequired();
            b.Property(x => x.CountryName).HasMaxLength(150).IsRequired();
        });

        modelBuilder.Entity<BusinessUnit>(b =>
        {
            b.ToTable("business_units");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.BusinessUnitCode).IsUnique();
            b.Property(x => x.BusinessUnitCode).HasMaxLength(50).IsRequired();
            b.Property(x => x.CountryCode).HasMaxLength(10).IsRequired();

            b.HasOne(x => x.Country)
                .WithMany(c => c.BusinessUnits)
                .HasForeignKey(x => x.CountryCode)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<SalesTerritory>(b =>
        {
            b.ToTable("sales_territories");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.TerritoryCode).IsUnique();
            b.Property(x => x.TerritoryCode).HasMaxLength(50).IsRequired();
            b.Property(x => x.CountryCode).HasMaxLength(10).IsRequired();

            b.HasOne(x => x.Country)
                .WithMany(c => c.SalesTerritories)
                .HasForeignKey(x => x.CountryCode)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<SalesOrganization>(b =>
        {
            b.ToTable("sales_organizations");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.SalesOrganizationCode).IsUnique();
            b.Property(x => x.SalesOrganizationCode).HasMaxLength(50).IsRequired();
            b.Property(x => x.DisplayName).HasMaxLength(150).IsRequired();
        });

        modelBuilder.Entity<BusinessUnitCountry>(b =>
        {
            b.ToTable("business_unit_countries");
            b.HasKey(x => new { x.BusinessUnitId, x.CountryCode });
        });
    }
}
