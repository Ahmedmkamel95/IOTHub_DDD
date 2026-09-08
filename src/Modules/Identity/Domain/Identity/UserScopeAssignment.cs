using CIOT.Common.Domain;

namespace CIOT.Modules.Identity.Domain.Entities;

public sealed class UserScopeAssignment : BaseEntity<Guid>, IEffectiveDatingEntity, ICreationAuditableEntity
{
    public Guid UserId { get; set; }
    public UserAccount User { get; set; } = null!;

    public string ScopeType { get; set; } = null!;
    public Guid? ScopeId { get; set; }
    public string? ScopeCode { get; set; }
    public string? CountryCode { get; set; }

    public DateTimeOffset ValidFromUtc { get; set; }
    public DateTimeOffset? ValidToUtc { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
}