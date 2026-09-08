using CIOT.Common.Domain;

namespace CIOT.Modules.Identity.Domain.Entities;

public sealed class AuthRevocationMarker : BaseEntity<Guid>, ICreationActorAuditableEntity
{
    public string MarkerType { get; set; } = null!;
    public byte[]? SessionIdHash { get; set; }
    public Guid? UserId { get; set; }
    public UserAccount? User { get; set; }
    public DateTimeOffset InvalidBeforeUtc { get; set; }
    public DateTimeOffset ExpiresAtUtc { get; set; }
    public string ReasonCode { get; set; } = null!;
    public string Source { get; set; } = null!;
    public Guid? CreatedByUserId { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
}