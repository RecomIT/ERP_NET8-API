using Shared.OtherModels.Pagination;

namespace Shared.Employee.Filter.Account
{
    public class EmployeeAccount_Filter : Sortparam
    {
        public string AccountInfoId { get; set; }
        public string EmployeeId { get; set; }
        public string PaymentMode { get; set; }
        public string BankId { get; set; }
        public string BankBranchId { get; set; }
        public string AgentName { get; set; }
        public string AccountNo { get; set; }
        public string IsActive { get; set; }
        public string StateStatus { get; set; }
    }
}
