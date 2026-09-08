using CIOT.Common.Data;
using CIOT.Modules.Admin.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.Admin.Infrastructure;

public sealed class AdminDbContext : BaseDbContext
{
    public const string Schema = "admin";

    public AdminDbContext(DbContextOptions<AdminDbContext> options) : base(options) { }

    public DbSet<AssetRecipe> AssetRecipes => Set<AssetRecipe>();
    public DbSet<AssetTargetDefault> AssetTargetDefaults => Set<AssetTargetDefault>();
    public DbSet<BpTargetConfiguration> BpTargetConfigurations => Set<BpTargetConfiguration>();
    public DbSet<DeviceModelControlCatalog> DeviceModelControlCatalogs => Set<DeviceModelControlCatalog>();
    public DbSet<EquipmentModel> EquipmentModels => Set<EquipmentModel>();
    public DbSet<EquipmentModelMedia> EquipmentModelMedias => Set<EquipmentModelMedia>();
    public DbSet<ErrorMapping> ErrorMappings => Set<ErrorMapping>();
    public DbSet<ExtractionQualityPolicy> ExtractionQualityPolicies => Set<ExtractionQualityPolicy>();
    public DbSet<OperationalStatusPolicy> OperationalStatusPolicies => Set<OperationalStatusPolicy>();
    public DbSet<RecipeConfiguration> RecipeConfigurations => Set<RecipeConfiguration>();
    public DbSet<RecipeTemplate> RecipeTemplates => Set<RecipeTemplate>();
    public DbSet<WaterFilterModel> WaterFilterModels => Set<WaterFilterModel>();
    public DbSet<VpmTarget> VpmTargets => Set<VpmTarget>();
 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schema);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AdminDbContext).Assembly);
    }
}
