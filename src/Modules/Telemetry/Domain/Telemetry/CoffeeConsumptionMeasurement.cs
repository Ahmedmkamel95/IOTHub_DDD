using CIOT.Common.Domain;

namespace CIOT.Modules.Telemetry.Domain.Entities;

public sealed class CoffeeConsumptionMeasurement : BaseEntity<Guid>, ICreationAuditableEntity
{
    public DateTimeOffset ConsumedAtUtc { get; set; }

    public Guid? RawMessageId { get; set; }

    public Guid? NormalizedMeasurementId { get; set; }

    public DateTimeOffset? NormalizedObservedAtUtc { get; set; }

    public Guid? SourceNormalizedEventId { get; set; }

    public DateTimeOffset? SourceNormalizedEventAtUtc { get; set; }

    public Guid? DeviceId { get; set; }

    public Guid? AssetId { get; set; }

    public Guid? OutletId { get; set; }

    public Guid? CustomerId { get; set; }

    public string? CountryCode { get; set; }

    public string? MachineType { get; set; }

    public decimal CoffeeKg { get; set; }

    public decimal? DoseGrams { get; set; }

    public decimal? CupsCount { get; set; }

    public string? ProductCode { get; set; }

    public Guid? MaterialId { get; set; }

    public string? RecipeCode { get; set; }

    public int? RecipeNumber { get; set; }

    public string? ButtonCode { get; set; }

    public int? Button { get; set; }

    public string? GroupCode { get; set; }

    public int? GroupNumber { get; set; }

    public string? DerivationMethod { get; set; }

    public string? DerivationVersion { get; set; }

    public string? DimensionsJson { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }
}