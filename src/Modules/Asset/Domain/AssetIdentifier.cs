using CIOT.Common.Domain;

namespace CIOT.Modules.Asset.Domain;

public sealed class AssetIdentifier : EntityBase
{
    public Guid AssetId { get; set; }
    public Asset Asset { get; set; } = null!;

    public string IdentifierType { get; set; } = null!; // e.g. "Barcode", "QR", "NFC"
    public string IdentifierValue { get; set; } = null!;
}
