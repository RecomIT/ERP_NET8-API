using Shared.OtherModels.Pagination;


namespace Shared.Asset.Filter.Resignation
{
    public class Resignation_Filter : Sortparam
    {
        public long? EmployeeId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Status { get; set; }
    }
}
