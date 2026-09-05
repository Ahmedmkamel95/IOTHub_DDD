using CIOT.Common.Domain;

namespace CIOT.Modules.CustomerOutlet.Domain;

public sealed class OutletNote : EntityBase
{
    public Guid OutletId { get; set; }
    public Outlet Outlet { get; set; } = null!;

    public Guid? RelatedAssetId { get; set; }
    public string NoteBody { get; set; } = null!;
    public bool IsDeleted { get; set; }
}
