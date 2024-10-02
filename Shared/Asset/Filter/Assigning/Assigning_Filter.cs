using Shared.OtherModels.Pagination;


namespace Shared.Asset.Filter.Assigning
{
    public class Assigning_Filter : Sortparam
    {
        public long AssigningId { get; set; }
        public long AssetId { get; set; } 
        public long? EmployeeId { get; set; }
        public string Status { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

    }
}
