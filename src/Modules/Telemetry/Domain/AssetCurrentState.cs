using CIOT.Common.Domain;

namespace CIOT.Modules.Telemetry.Domain;

public sealed class AssetCurrentState : AggregateRoot
{
    public Guid AssetId { get; set; }
    public Guid? DeviceId { get; set; }
    public DateTime LastTelemetryAtUtc { get; set; }
    public string? MachineStatus { get; set; } // "Ready", "Brewing", "Error", "Standby"
    public decimal? WaterLitersToday { get; set; }
    public decimal? EnergyKwhToday { get; set; }
    public decimal? CoffeeKgToday { get; set; }
    public int? CupsToday { get; set; }
    public decimal? ConnectivityQualityScore { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? StateJson { get; set; }
}

