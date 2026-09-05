using CIOT.Common.Domain;

namespace CIOT.Modules.Mobile.Domain;

public sealed class OfflineActionResult : EntityBase
{
    public Guid OfflineBatchId { get; set; }
    public string ClientActionId { get; set; } = null!;
    public string ActionType { get; set; } = null!; // "ReplaceDevice", "InstallFilter", "ChangeDosage"
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
}
