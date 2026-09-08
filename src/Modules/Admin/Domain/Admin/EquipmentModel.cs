using CIOT.Common.Domain;

namespace CIOT.Modules.Admin.Domain.Entities;

public sealed class EquipmentModel : BaseEntity<Guid>,
ICreationAuditableEntity,
IUpdateAuditableEntity,
IOptimisticConcurrentEntity
{
    public Guid? AssetManufacturerId { get; set; }
    public string Manufacturer { get; set; } = default!;
    public string Model { get; set; } = default!;
    public string? Submodel { get; set; }

    public string MachineType { get; set; } = default!;
    public string? MachineCategory { get; set; }

    public string? TelemetryCapabilitiesJson { get; set; }

    public bool SupportsPhysicalDevice { get; set; } = true;
    public bool SupportsDevicelessTelemetry { get; set; } 

    public string? DefaultPartnerSource { get; set; }
    public string? DevicelessIdentityType { get; set; }

    public string? DefaultRecipeJson { get; set; }

    public string Status { get; set; } = default!;

    public decimal? MonthlyCoffeeTargetKg { get; set; }
    public decimal? MonthlyChocoTargetKg { get; set; }
    public int? MonthlyCupTarget { get; set; }
    public int? TargetCupsPerDay { get; set; }
    public long RowVersion { get; set; } = 1;

    public DateTimeOffset CreatedAtUtc { get; set; }
    public Guid? CreatedByUserId { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
    public Guid? UpdatedByUserId { get; set; }
}