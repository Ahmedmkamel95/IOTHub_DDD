using CIOT.Common.Domain;

namespace CIOT.Modules.Provisioning.Domain.Entities;

public sealed class TelemetryDictionaryPackage : BaseEntity<Guid>, ICreationAuditableEntity, IOptimisticConcurrentEntity
{
    public int DictionaryVersion { get; set; }
    public string SchemaName { get; set; } = default!;
    public int SchemaVersion { get; set; }
    public string Status { get; set; } = default!;
    public string PackageHash { get; set; } = default!;
    public string DictionaryJson { get; set; } = default!;
    public DateTimeOffset? PublishedAtUtc { get; set; }
    public Guid? PublishedByUserId { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? RetiredAtUtc { get; set; }
    public Guid? RetiredByUserId { get; set; }
    public long RowVersion { get; set; }
}