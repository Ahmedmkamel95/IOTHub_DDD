using CIOT.Common.Domain;
using NetTopologySuite.Geometries;

namespace CIOT.Modules.Telemetry.Domain.Entities;

public sealed class AssetCurrentState : IUpdateAuditableEntity
{
    public Guid AssetId { get; set; }
    public Guid? DeviceId { get; set; }
    public Guid? OutletId { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CountryCode { get; set; }
    public string? MachineType { get; set; }

    public DateTimeOffset? LastObservedAtUtc { get; set; }
    public DateTimeOffset LastTelemetryAtUtc { get; set; }
    public Guid? LastRawMessageId { get; set; }

    public string? MachineStatus { get; set; }
    public string? MachineMode { get; set; }
    public string? ConnectionStatus { get; set; }
    public bool? IsOnline { get; set; }
    public DateTimeOffset? LastCommunicationAtUtc { get; set; }
    public string? ConnectionType { get; set; }
    public decimal? SignalQuality { get; set; }
    public int? RssiDbm { get; set; }

    public Point? Location { get; set; }
    public DateTimeOffset? LocationReportedAtUtc { get; set; }
    public string? FirmwareVersion { get; set; }
    public string? CurrentPayloadJson { get; set; }

    public decimal? WaterLitersToday { get; set; }
    public decimal? EnergyKwhToday { get; set; }
    public decimal? CoffeeKgToday { get; set; }
    public int? CupsToday { get; set; }
    public decimal? ConnectivityQualityScore { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? StateJson { get; set; }

    public DateTimeOffset? UpdatedAtUtc { get; set; }
}