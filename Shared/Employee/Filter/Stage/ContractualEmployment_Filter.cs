using Shared.OtherModels.Pagination;

namespace Shared.Employee.Filter.Stage
{
    public class ContractualEmployment_Filter : Sortparam
    {
        public string ContractId { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string ContractCode { get; set; }
        public string StateStatus { get; set; }
    }
}
