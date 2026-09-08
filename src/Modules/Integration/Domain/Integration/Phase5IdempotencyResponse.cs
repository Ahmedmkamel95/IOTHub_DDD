using CIOT.Common.Domain;

namespace CIOT.Modules.Integration.Domain.Entities;

public sealed class Phase5IdempotencyResponse : BaseEntity<Guid>, ICreationAuditableEntity
{
    public Guid CallerUserId { get; set; }
    public string OperationId { get; set; } = null!;
    public string IdempotencyKey { get; set; } = null!;
    public string RequestHash { get; set; } = null!;
    public string State { get; set; } = null!;
    public int? ResponseStatus { get; set; }
    public string? ResponseContentType { get; set; }
    public byte[]? ResponseBody { get; set; }
    public string? ResponseLocation { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? CompletedAtUtc { get; set; }
    public DateTimeOffset ExpiresAtUtc { get; set; }
}