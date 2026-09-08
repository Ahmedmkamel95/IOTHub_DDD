using CIOT.Common.Domain;

namespace CIOT.Modules.Identity.Domain.Entities;

public sealed class MobileProfilePreference : IOptimisticConcurrentEntity, ICreationAuditableEntity, IUpdateAuditableEntity
{
    public Guid UserId { get; set; }
    public string Environment { get; set; } = null!;
    public bool PushNotificationsEnabled { get; set; }
    public bool EmailNotificationsEnabled { get; set; }
    public long RowVersion { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
}