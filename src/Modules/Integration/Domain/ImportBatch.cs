using CIOT.Common.Domain;

namespace CIOT.Modules.Integration.Domain;

public sealed class ImportBatch : AggregateRoot
{
    public string BatchReference { get; set; } = Guid.NewGuid().ToString();
    public string EntityType { get; set; } = null!; // "Customer", "Outlet", "Asset"
    public int TotalRecords { get; set; }
    public int SuccessRecords { get; set; }
    public int FailedRecords { get; set; }
    public string Status { get; set; } = "Completed";
}
