using CIOT.Common.Domain;

namespace CIOT.Modules.Telemetry.Domain.Entities;

public sealed class ConnectivityReport : BaseEntity<Guid>, ICreationAuditableEntity
{
    public DateTimeOffset ReportedAtUtc { get; set; }

    public Guid? RawMessageId { get; set; }

    public Guid? NormalizedEventId { get; set; }

    public DateTimeOffset? NormalizedEventAtUtc { get; set; }

    public Guid? NormalizedMeasurementId { get; set; }

    public DateTimeOffset? NormalizedObservedAtUtc { get; set; }

    public Guid DeviceId { get; set; }

    public Guid? AssetId { get; set; }

    public Guid? OutletId { get; set; }

    public Guid? CustomerId { get; set; }

    public string? CountryCode { get; set; }

    public string? MachineType { get; set; }

    public string ConnectionType { get; set; } = default!;

    public string? ConnectionStatus { get; set; }

    public int? RssiDbm { get; set; }

    public int? RsrpDbm { get; set; }

    public decimal? RsrqDb { get; set; }

    public decimal? SinrDb { get; set; }

    public decimal? SignalQuality { get; set; }

    public string? RadioAccessTechnology { get; set; }

    public string? CellId { get; set; }

    public string? OperatorName { get; set; }

    public string? WifiSsidHash { get; set; }

    public string? IpAddress { get; set; }

    public string? DiagnosticsJson { get; set; }

    public string GeolocationStatus { get; set; } = default!;

    public string? GeolocationProvider { get; set; }

    public int GeolocationAttemptCount { get; set; }

    public DateTimeOffset? GeolocationLastAttemptAtUtc { get; set; }

    public DateTimeOffset? GeolocationNextAttemptAtUtc { get; set; }

    public Guid? GeolocationResolvedLocationReportId { get; set; }

    public string? GeolocationRequestHash { get; set; }

    public string? GeolocationInputSummaryJson { get; set; }

    public string? GeolocationErrorCode { get; set; }

    public string? GeolocationErrorMessage { get; set; }

    public string? GeolocationLockedBy { get; set; }

    public DateTimeOffset? GeolocationLockedUntilUtc { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }

    public ICollection<LocalLocationResolutionQueue> LocalLocationResolutionQueues { get; } =
        new List<LocalLocationResolutionQueue>();
}