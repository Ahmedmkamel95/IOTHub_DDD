using CIOT.Modules.Admin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CIOT.Modules.Admin.Infrastructure;

internal sealed class AssetRecipeConfiguration : IEntityTypeConfiguration<AssetRecipe>
{
    public void Configure(EntityTypeBuilder<AssetRecipe> b)
    {
        b.ToTable("asset_recipes"); b.HasKey(x => x.Id);
        b.Property(x => x.RecipeCode).HasMaxLength(100).IsRequired(); b.Property(x => x.DosageJson).IsRequired(); b.Property(x => x.DeploymentStatus).HasMaxLength(50).IsRequired();
        b.HasIndex(x => new { x.AssetId, x.IsCurrent }); b.HasIndex(x => new { x.RecipeTemplateId, x.RecipeCode });
        b.HasOne(x => x.RecipeTemplate).WithMany().HasForeignKey(x => x.RecipeTemplateId).OnDelete(DeleteBehavior.SetNull);
    }
}

internal sealed class AssetTargetDefaultConfiguration : IEntityTypeConfiguration<AssetTargetDefault>
{
    public void Configure(EntityTypeBuilder<AssetTargetDefault> b)
    {
        b.ToTable("asset_target_defaults"); b.HasKey(x => x.Id); b.Property(x => x.CountryCode).HasMaxLength(10).IsRequired(); b.Property(x => x.MetricKey).HasMaxLength(100).IsRequired(); b.Property(x => x.Unit).HasMaxLength(50).IsRequired();
        b.HasIndex(x => new { x.CountryCode, x.AssetTypeCode, x.EquipmentModelId, x.MetricKey, x.ValidFromUtc });
        b.HasOne(x => x.EquipmentModel).WithMany().HasForeignKey(x => x.EquipmentModelId).OnDelete(DeleteBehavior.SetNull);
    }
}

internal sealed class BpTargetConfigurationConfiguration : IEntityTypeConfiguration<BpTargetConfiguration>
{
    public void Configure(EntityTypeBuilder<BpTargetConfiguration> b)
    {
        b.ToTable("bp_target_configurations"); b.HasKey(x => x.Id); b.Property(x => x.CountryCode).HasMaxLength(10).IsRequired(); b.Property(x => x.Category).HasMaxLength(100).IsRequired(); b.Property(x => x.Status).HasMaxLength(50).IsRequired();
        b.HasIndex(x => new { x.TargetYear, x.BusinessUnitId, x.CountryCode, x.Category }).IsUnique();
    }
}

internal sealed class DeviceModelControlCatalogConfiguration : IEntityTypeConfiguration<DeviceModelControlCatalog>
{
    public void Configure(EntityTypeBuilder<DeviceModelControlCatalog> b)
    {
        b.ToTable("device_model_control_catalogs"); b.HasKey(x => x.Id); b.Property(x => x.ControlKey).HasMaxLength(100).IsRequired(); b.Property(x => x.ControlKind).HasMaxLength(50).IsRequired(); b.Property(x => x.JsonSchema).IsRequired(); b.Property(x => x.RequiredPermissionCode).HasMaxLength(100).IsRequired(); b.Property(x => x.RiskLevel).HasMaxLength(50).IsRequired(); b.Property(x => x.Status).HasMaxLength(50).IsRequired();
        b.HasIndex(x => new { x.EquipmentModelId, x.ControlKey, x.ContractVersion }).IsUnique(); b.HasOne(x => x.EquipmentModel).WithMany().HasForeignKey(x => x.EquipmentModelId).OnDelete(DeleteBehavior.Cascade);
    }
}

internal sealed class EquipmentModelConfiguration : IEntityTypeConfiguration<EquipmentModel>
{
    public void Configure(EntityTypeBuilder<EquipmentModel> b)
    {
        b.ToTable("equipment_models"); b.HasKey(x => x.Id); b.Property(x => x.Manufacturer).HasMaxLength(150).IsRequired(); b.Property(x => x.Model).HasMaxLength(150).IsRequired(); b.Property(x => x.MachineType).HasMaxLength(100).IsRequired(); b.Property(x => x.Status).HasMaxLength(50).IsRequired(); b.HasIndex(x => new { x.Manufacturer, x.Model }).IsUnique();
    }
}

internal sealed class EquipmentModelMediaConfiguration : IEntityTypeConfiguration<EquipmentModelMedia>
{
    public void Configure(EntityTypeBuilder<EquipmentModelMedia> b)
    { b.ToTable("equipment_model_medias"); b.HasKey(x => x.Id); b.Property(x => x.MediaKind).HasMaxLength(50).IsRequired(); b.Property(x => x.ObjectStorageUri).HasMaxLength(1000).IsRequired(); b.Property(x => x.ContentType).HasMaxLength(150).IsRequired(); b.Property(x => x.ContentSha256).HasMaxLength(64).IsRequired(); b.Property(x => x.MalwareScanStatus).HasMaxLength(50).IsRequired(); b.Property(x => x.Status).HasMaxLength(50).IsRequired(); b.HasIndex(x => new { x.EquipmentModelId, x.MediaKind }); b.HasOne(x => x.EquipmentModel).WithMany().HasForeignKey(x => x.EquipmentModelId).OnDelete(DeleteBehavior.Cascade); }
}

internal sealed class ErrorMappingConfiguration : IEntityTypeConfiguration<ErrorMapping>
{
    public void Configure(EntityTypeBuilder<ErrorMapping> b)
    { b.ToTable("error_mappings"); b.HasKey(x => x.Id); b.Property(x => x.Manufacturer).HasMaxLength(150).IsRequired(); b.Property(x => x.SourceErrorCode).HasMaxLength(100).IsRequired(); b.Property(x => x.Severity).HasMaxLength(50).IsRequired(); b.Property(x => x.DisplayMessage).HasMaxLength(1000).IsRequired(); b.HasIndex(x => new { x.Manufacturer, x.SourceErrorCode, x.Model, x.CountryCode }); }
}

internal sealed class ExtractionQualityPolicyConfiguration : IEntityTypeConfiguration<ExtractionQualityPolicy>
{
    public void Configure(EntityTypeBuilder<ExtractionQualityPolicy> b)
    { b.ToTable("extraction_quality_policies"); b.HasKey(x => x.Id); b.Property(x => x.MeasurementCode).HasMaxLength(100).IsRequired(); b.Property(x => x.MeasurementUnit).HasMaxLength(50).IsRequired(); b.Property(x => x.Status).HasMaxLength(50).IsRequired(); b.HasIndex(x => new { x.EquipmentModelId, x.CountryCode, x.MeasurementCode, x.Priority, x.ValidFromUtc }); b.HasOne(x => x.EquipmentModel).WithMany().HasForeignKey(x => x.EquipmentModelId).OnDelete(DeleteBehavior.SetNull); }
}

internal sealed class OperationalStatusPolicyConfiguration : IEntityTypeConfiguration<OperationalStatusPolicy>
{
    public void Configure(EntityTypeBuilder<OperationalStatusPolicy> b)
    { b.ToTable("operational_status_policies"); b.HasKey(x => x.PolicyCode); b.Property(x => x.PolicyCode).HasMaxLength(100).IsRequired(); b.Property(x => x.Status).HasMaxLength(50).IsRequired(); b.HasIndex(x => x.PolicyCode).IsUnique(); }
}

internal sealed class RecipeConfigurationConfiguration : IEntityTypeConfiguration<RecipeConfiguration>
{
    public void Configure(EntityTypeBuilder<RecipeConfiguration> b)
    { b.ToTable("recipe_configurations"); b.HasKey(x => x.Id); b.Property(x => x.CountryCode).HasMaxLength(10).IsRequired(); b.Property(x => x.Status).HasMaxLength(50).IsRequired(); b.HasIndex(x => new { x.CountryCode, x.EquipmentModelId }).IsUnique(); b.HasOne(x => x.EquipmentModel).WithMany().HasForeignKey(x => x.EquipmentModelId).OnDelete(DeleteBehavior.Cascade); }
}

internal sealed class RecipeTemplateConfiguration : IEntityTypeConfiguration<RecipeTemplate>
{
    public void Configure(EntityTypeBuilder<RecipeTemplate> b)
    { b.ToTable("recipe_templates"); b.HasKey(x => x.Id); b.Property(x => x.CountryCode).HasMaxLength(10).IsRequired(); b.Property(x => x.RecipeCode).HasMaxLength(100).IsRequired(); b.Property(x => x.RecipeName).HasMaxLength(200).IsRequired(); b.Property(x => x.DosageJson).IsRequired(); b.Property(x => x.Status).HasMaxLength(50).IsRequired(); b.HasIndex(x => new { x.CountryCode, x.EquipmentModelId, x.RecipeCode }).IsUnique(); b.HasOne(x => x.EquipmentModel).WithMany().HasForeignKey(x => x.EquipmentModelId).OnDelete(DeleteBehavior.Cascade); b.HasOne(x => x.RecipeConfiguration).WithOne().HasForeignKey<RecipeTemplate>(x => x.RecipeConfigurationId).OnDelete(DeleteBehavior.SetNull); }
}

internal sealed class WaterFilterModelConfiguration : IEntityTypeConfiguration<WaterFilterModel>
{
    public void Configure(EntityTypeBuilder<WaterFilterModel> b)
    { b.ToTable("water_filter_models"); b.HasKey(x => x.Id); b.Property(x => x.Model).HasMaxLength(150).IsRequired(); b.Property(x => x.CountryCode).HasMaxLength(10); b.HasIndex(x => new { x.Manufacturer, x.Model, x.CountryCode }).IsUnique(); }
}

internal sealed class VpmTargetEntityConfiguration : IEntityTypeConfiguration<VpmTarget>
{
    public void Configure(EntityTypeBuilder<VpmTarget> b)
    { b.ToTable("vpm_targets"); b.HasKey(x => x.Id); b.Property(x => x.CountryCode).HasMaxLength(10).IsRequired(); b.Property(x => x.MetricKey).HasMaxLength(100).IsRequired(); b.Property(x => x.Unit).HasMaxLength(50).IsRequired(); b.HasIndex(x => new { x.TargetYear, x.TargetMonth, x.CountryCode, x.EquipmentModelId, x.MetricKey }); b.HasOne(x => x.TargetConfiguration).WithMany().HasForeignKey(x => x.TargetConfigurationId).OnDelete(DeleteBehavior.SetNull); b.HasOne(x => x.EquipmentModel).WithMany().HasForeignKey(x => x.EquipmentModelId).OnDelete(DeleteBehavior.SetNull); }
}

internal sealed class VpmTargetConfigurationEntityConfiguration : IEntityTypeConfiguration<VpmTargetConfiguration>
{
    public void Configure(EntityTypeBuilder<VpmTargetConfiguration> b)
    { b.ToTable("vpm_target_configurations"); b.HasKey(x => x.Id); b.Property(x => x.CountryCode).HasMaxLength(10).IsRequired(); b.Property(x => x.Category).HasMaxLength(100).IsRequired(); b.Property(x => x.MachineType).HasMaxLength(100).IsRequired(); b.Property(x => x.Manufacturer).HasMaxLength(150).IsRequired(); b.Property(x => x.Model).HasMaxLength(150).IsRequired(); b.Property(x => x.Status).HasMaxLength(50).IsRequired(); b.HasIndex(x => new { x.TargetYear, x.CountryCode, x.Category, x.MachineType, x.Manufacturer, x.Model }).IsUnique(); b.HasOne(x => x.EquipmentModel).WithMany().HasForeignKey(x => x.EquipmentModelId).OnDelete(DeleteBehavior.SetNull); }
}
