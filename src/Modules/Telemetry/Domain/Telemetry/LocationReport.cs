using CIOT.Common.Domain;
using System.Drawing;

namespace CIOT.Modules.Telemetry.Domain.Entities;

public sealed class LocationReport : BaseEntity<Guid>, ICreationAuditableEntity, ILocatibleEntity
{

    public DateTimeOffset ReportedAtUtc { get; set; }

    public Guid? RawMessageId { get; set; }

    public Guid? NormalizedEventId { get; set; }

    public DateTimeOffset? NormalizedEventAtUtc { get; set; }

    public Guid? DeviceId { get; set; }

    public Guid? AssetId { get; set; }

    public Guid? OutletId { get; set; }

    public Guid? CustomerId { get; set; }

    public string? CountryCode { get; set; }

    public string? MachineType { get; set; }

    public Point Location { get; set; } = default!;

    public decimal? AccuracyMeters { get; set; }

    public decimal? AltitudeMeters { get; set; }

    public string? Source { get; set; }

    public Guid? SourceConnectivityReportId { get; set; }

    public DateTimeOffset? SourceConnectivityReportedAtUtc { get; set; }

    public string? Provider { get; set; }

    public string? ProviderRequestHash { get; set; }

    public string? ResolutionMethod { get; set; }

    public string? ResolutionMetadataJson { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }
}