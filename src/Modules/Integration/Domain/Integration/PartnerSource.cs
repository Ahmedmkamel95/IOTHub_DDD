using CIOT.Common.Domain;

namespace CIOT.Modules.Integration.Domain.Entities;

public sealed class PartnerSource : BaseEntity<Guid>, ICreationAuditableEntity, IUpdateAuditableEntity
{
    public string PartnerCode { get; set; } = null!;
    public string SourceCode { get; set; } = null!;
    public string SourceType { get; set; } = null!;
    public string ImplementationWorker { get; set; } = null!;
    public string? ScheduleExpression { get; set; }
    public string? CredentialSecretReference { get; set; }
    public string EndpointReference { get; set; } = null!;
    public string CheckpointFeedCode { get; set; } = null!;
    public string IdentityType { get; set; } = null!;
    public bool Enabled { get; set; }
    public string ConfigurationJson { get; set; } = null!;
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
}