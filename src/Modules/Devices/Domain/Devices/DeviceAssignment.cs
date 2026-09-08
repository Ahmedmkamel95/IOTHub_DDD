using CIOT.Common.Domain;

namespace CIOT.Modules.Devices.Domain.Entities;

public sealed class DeviceAssignment : BaseEntity<Guid>, ICreationAuditableEntity
{
    public Guid DeviceId { get; set; }
    public Device Device { get; set; } = null!;

    public Guid AssetId { get; set; }

    public string AssignmentType { get; set; } = default!;

    public DateTimeOffset PairedAtUtc { get; set; }
    public DateTimeOffset? UnpairedAtUtc { get; set; }

    public Guid? PairedByUserId { get; set; }
    public Guid? UnpairedByUserId { get; set; }

    public string? AppName { get; set; }
    public string? AppVersion { get; set; }

    public bool FactoryAssociation { get; set; }

    public string? EvidenceJson { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }

    public FotaUpdateSession? FotaUpdateSession { get; set; }
    public ICollection<FotaDownloadGrant> FotaDownloadGrants { get; } =
        new List<FotaDownloadGrant>();
}