using CIOT.Common.Domain;

namespace CIOT.Modules.Asset.Domain;

public sealed class Asset : AggregateRoot
{
    public string SapEquipmentNumber { get; set; } = null!;
    public string? OemSerialNumber { get; set; }
    public string? TechnicalId { get; set; }

    public Guid? AssetTypeId { get; set; }
    public Guid? EquipmentModelId { get; set; }
    public string CountryCode { get; set; } = null!;
    public string? SapStatus { get; set; }

    public DateOnly? ActivationDate { get; set; }
    public DateOnly? AcquisitionDate { get; set; }
    public DateTime? LastConnectionAtUtc { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<AssetOutletAssignment> OutletAssignments { get; } = new List<AssetOutletAssignment>();
    public ICollection<AssetWaterFilter> WaterFilters { get; } = new List<AssetWaterFilter>();
    public ICollection<AssetIdentifier> Identifiers { get; } = new List<AssetIdentifier>();

    public AssetOutletAssignment AssignToCustomerOutlet(Guid? customerId, Guid? outletId)
    {
        foreach (var current in OutletAssignments.Where(a => a.IsCurrent))
        {
            current.Unassign();
        }

        var assignment = new AssetOutletAssignment
        {
            AssetId = Id,
            CustomerId = customerId,
            OutletId = outletId,
            AssignedAtUtc = DateTime.UtcNow,
            IsCurrent = true
        };

        OutletAssignments.Add(assignment);
        return assignment;
    }
}

