using CIOT.Common.Domain;

namespace CIOT.Modules.Telemetry.Domain.Entities;

public sealed class EnergyConsumptionMeasurement : BaseEntity<Guid>, ICreationAuditableEntity
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

    public decimal EnergyKwh { get; set; }

    public decimal? PowerW { get; set; }

    public DateTimeOffset? PeriodStartUtc { get; set; }

    public DateTimeOffset? PeriodEndUtc { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }
}