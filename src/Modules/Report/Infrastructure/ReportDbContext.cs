using CIOT.Common.Data;
using CIOT.Modules.Report.Domain;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.Report.Infrastructure;

public sealed class ReportDbContext : BaseDbContext
{
    public const string Schema = "report";

    public ReportDbContext(DbContextOptions<ReportDbContext> options) : base(options) { }

    public DbSet<ReportDefinition> ReportDefinitions => Set<ReportDefinition>();
    public DbSet<ReportRun> ReportRuns => Set<ReportRun>();
    public DbSet<ExportJob> ExportJobs => Set<ExportJob>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schema);

        modelBuilder.Entity<ReportDefinition>(b =>
        {
            b.ToTable("report_definitions");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.ReportCode).IsUnique();
        });

        modelBuilder.Entity<ReportRun>(b =>
        {
            b.ToTable("report_runs");
            b.HasKey(x => x.Id);
            b.HasIndex(x => new { x.ReportDefinitionId, x.Status });
        });

        modelBuilder.Entity<ExportJob>(b =>
        {
            b.ToTable("export_jobs");
            b.HasKey(x => x.Id);
        });
    }
}
