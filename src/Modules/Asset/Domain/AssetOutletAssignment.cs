using CIOT.Common.Domain;

namespace CIOT.Modules.Asset.Domain;

public sealed class AssetOutletAssignment : EntityBase
{
    public Guid AssetId { get; set; }
    public Asset Asset { get; set; } = null!;

    public Guid? OutletId { get; set; } // References CustomerOutlet.Outlet
    public Guid? CustomerId { get; set; }

    public DateTime AssignedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UnassignedAtUtc { get; set; }

    public bool IsCurrent { get; set; } = true;

    public void Unassign()
    {
        IsCurrent = false;
        UnassignedAtUtc = DateTime.UtcNow;
    }
}
