using CIOT.Common.Domain;

namespace CIOT.Modules.Telemetry.Domain.Entities;

public sealed class IngredientConsumptionMeasurement : BaseEntity<Guid>, ICreationAuditableEntity
{
    public DateTimeOffset MeasuredAtUtc { get; set; }

    public Guid? RawMessageId { get; set; }

    public Guid? NormalizedMeasurementId { get; set; }

    public DateTimeOffset? NormalizedObservedAtUtc { get; set; }

    public Guid? DeviceId { get; set; }

    public Guid? AssetId { get; set; }

    public Guid? OutletId { get; set; }

    public Guid? CustomerId { get; set; }

    public string? CountryCode { get; set; }

    public string? MachineType { get; set; }

    public string MetricKey { get; set; } = default!;

    public string Unit { get; set; } = default!;

    public decimal QuantityNumeric { get; set; }

    public string? ProductCode { get; set; }

    public string? IngredientCode { get; set; }

    public Guid? MaterialId { get; set; }

    public string? RecipeCode { get; set; }

    public int? RecipeNumber { get; set; }

    public string? ButtonCode { get; set; }

    public int? Button { get; set; }

    public string? GroupCode { get; set; }

    public int? GroupNumber { get; set; }

    public string? ContainerCode { get; set; }

    public string? CounterKind { get; set; }

    public DateTimeOffset? PeriodStartUtc { get; set; }

    public DateTimeOffset? PeriodEndUtc { get; set; }

    public string? DimensionsJson { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }
}