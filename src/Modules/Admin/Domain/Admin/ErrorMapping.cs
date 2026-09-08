using CIOT.Common.Domain;

namespace CIOT.Modules.Admin.Domain.Entities;

public sealed class ErrorMapping : BaseEntity<Guid> , IUpdateActorAuditableEntity, ICreationActorAuditableEntity , IOptimisticConcurrentEntity
{
    public string Manufacturer { get; set; } = default!;

    public string? Model { get; set; }

    public string? CountryCode { get; set; }
    public string SourceErrorCode { get; set; } = default!;

    public string Severity { get; set; } = default!;

    public string DisplayMessage { get; set; } = default!;

    public string? RecommendedAction { get; set; }

    public bool IsActive { get; set; } = true;
    public long RowVersion { get; set; } = 1;

    public DateTimeOffset CreatedAtUtc { get; set; }
    public Guid? CreatedByUserId { get; set; }

    public DateTimeOffset? UpdatedAtUtc { get; set; }
    public Guid? UpdatedByUserId { get; set; }
}