using Shared.OtherModels.Pagination;


namespace Shared.Asset.Filter.Support
{
    public class Replacement_Filter : Sortparam
    {
        public long? EmployeeId { get; set; }
        public string Status { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

    }
}
