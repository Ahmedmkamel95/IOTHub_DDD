using CIOT.Common.Domain;

namespace CIOT.Modules.Identity.Domain.Entities;

public sealed class MobileTermsDocument : BaseEntity<Guid>, ICreationAuditableEntity
{
    public string TermsVersion { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string ContentUrl { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTimeOffset? PublishedAtUtc { get; set; }
    public DateTimeOffset? RetiredAtUtc { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
}