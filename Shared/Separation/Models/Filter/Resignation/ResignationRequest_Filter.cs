using Shared.OtherModels.Pagination;

namespace Shared.Separation.Models.Filter.Resignation
{
    public class ResignationRequest_Filter : Pageparam
    {
        public string ResignationId { get; set; }
        public string ResignCode { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string ResignationReason { get; set; }
        public string StateStatus { get; set; }
    }
}
