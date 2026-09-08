using CIOT.Common.Domain;

namespace CIOT.Modules.Admin.Domain.Entities;

public sealed class AssetRecipe : BaseEntity<Guid> , IEffectiveDatingEntity , ICreationActorAuditableEntity
{
    public Guid AssetId { get; set; }

    public Guid? RecipeTemplateId { get; set; }
    public RecipeTemplate RecipeTemplate { get; set; } = default!;
    public string RecipeCode { get; set; } = default!;

    public string DosageJson { get; set; } = default!;
    public bool IsCurrent { get; set; } = true;

    public DateTimeOffset ValidFromUtc { get; set; }
    public DateTimeOffset? ValidToUtc { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }
    public Guid? CreatedByUserId { get; set; }

    // Deployment tracking
    public string? DesiredPayloadSha256 { get; set; }

    public string DeploymentStatus { get; set; } = default!;

    public DateTimeOffset? AcknowledgedAtUtc { get; set; }

    public string? AcknowledgementCorrelationId { get; set; }

    public string? FailureCode { get; set; }
}