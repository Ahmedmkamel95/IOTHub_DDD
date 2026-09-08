using CIOT.Common.Domain;

namespace CIOT.Modules.Identity.Domain.Entities;

public sealed class UserActivation : BaseEntity<Guid>, ICreationActorAuditableEntity
{
    public Guid UserId { get; set; }
    public UserAccount User { get; set; } = null!;
    public string AuthProvider { get; set; } = null!;
    public string TokenHash { get; set; } = null!;
    public string InvitedEmail { get; set; } = null!;
    public string ReturnPath { get; set; } = null!;
    public DateTimeOffset ExpiresAtUtc { get; set; }
    public DateTimeOffset? ConsumedAtUtc { get; set; }
    public DateTimeOffset? RevokedAtUtc { get; set; }
    public Guid? CreatedByUserId { get; set; }
    public int SendCount { get; set; }
    public DateTimeOffset? LastSentAtUtc { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
}