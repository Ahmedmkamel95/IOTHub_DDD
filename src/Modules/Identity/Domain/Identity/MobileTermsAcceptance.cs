using CIOT.Common.Domain;

namespace CIOT.Modules.Identity.Domain.Entities;

public sealed class MobileTermsAcceptance : BaseEntity<Guid>, ICreationAuditableEntity
{
    public Guid UserId { get; set; }
    public Guid TermsDocumentId { get; set; }
    public DateTimeOffset AcceptedAtUtc { get; set; }
    public string? AppVersion { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
}