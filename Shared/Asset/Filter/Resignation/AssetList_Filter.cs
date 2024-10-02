using Shared.OtherModels.Pagination;


namespace Shared.Asset.Filter.Resignation
{
    public class AssetList_Filter : Sortparam
    {
        public long? AssigningId { get; set; }
        public long? EmployeeId { get; set; }
    }

}
