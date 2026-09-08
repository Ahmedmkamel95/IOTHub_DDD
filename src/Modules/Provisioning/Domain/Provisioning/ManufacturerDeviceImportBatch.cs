using CIOT.Common.Domain;

namespace CIOT.Modules.Provisioning.Domain.Entities;

public sealed class ManufacturerDeviceImportBatch : BaseEntity<Guid>
{
    public Guid DeviceManufacturerId { get; set; }
    public Guid? SubmittedByUserId { get; set; }
    public Guid? SubmittedByApiClientId { get; set; }
    public string IdempotencyKey { get; set; }
    public string? FileName { get; set; }
    public string PayloadSha256 { get; set; }
    public string Status { get; set; }
    public int TotalCount { get; set; }
    public int AcceptedCount { get; set; }
    public int RejectedCount { get; set; }
    public DateTimeOffset SubmittedAtUtc { get; set; }
    public DateTimeOffset? CompletedAtUtc { get; set; }
}