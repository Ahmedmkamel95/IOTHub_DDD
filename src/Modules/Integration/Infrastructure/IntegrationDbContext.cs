using CIOT.Common.Data;
using CIOT.Modules.Integration.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.Integration.Infrastructure;

public sealed class IntegrationDbContext : BaseDbContext
{
    public const string Schema = "integration";

    public IntegrationDbContext(DbContextOptions<IntegrationDbContext> options) : base(options) { }

    public DbSet<PartnerSource> PartnerSources => Set<PartnerSource>();
    public DbSet<ImportBatch> ImportBatches => Set<ImportBatch>();
    public DbSet<PartnerRawOutbox> PartnerRawOutboxes => Set<PartnerRawOutbox>();
    public DbSet<PartnerCheckpoint> PartnerCheckpoints => Set<PartnerCheckpoint>();
    public DbSet<LocalPartnerRawQueue> LocalPartnerRawQueues => Set<LocalPartnerRawQueue>();
    public DbSet<ImportBatchItem> ImportBatchItems => Set<ImportBatchItem>();
    public DbSet<PartnerSourceCheckpoint> PartnerSourceCheckpoints => Set<PartnerSourceCheckpoint>();
    public DbSet<PartnerIngestionWorkflow> PartnerIngestionWorkflows => Set<PartnerIngestionWorkflow>();
    public DbSet<Phase5IdempotencyResponse> Phase5IdempotencyResponses => Set<Phase5IdempotencyResponse>();
    public DbSet<SapOrderBlockPolicy> SapOrderBlockPolicies => Set<SapOrderBlockPolicy>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IntegrationDbContext).Assembly);

        modelBuilder.Entity<PartnerSource>(b =>
        {
            b.ToTable("partner_sources");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.SourceCode).IsUnique();
        });

        modelBuilder.Entity<ImportBatch>(b =>
        {
            b.ToTable("import_batches");
            b.HasKey(x => x.Id);
            b.HasIndex(x => new { x.SourceSystem, x.SourceBatchId }).IsUnique();
        });

        modelBuilder.Entity<PartnerRawOutbox>(b =>
        {
            b.ToTable("partner_raw_outboxes");
            b.HasKey(x => x.Id);
            b.HasIndex(x => new { x.Status, x.CreatedAtUtc });
        });
    }
}
