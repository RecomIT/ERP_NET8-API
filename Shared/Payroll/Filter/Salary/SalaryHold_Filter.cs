using Shared.OtherModels.Pagination;

namespace Shared.Payroll.Filter.Salary
{
    public class SalaryHold_Filter : Pageparam
    {
        public string EmployeeId { get; set; }
        public string SalaryHoldId { get; set; }
        public string EmployeeCode { get; set; }
        public string SalaryMonth { get; set; }
        public string SalaryYear { get; set; }
        public string HoldReason { get; set; }
        public string StateStatus { get; set; }
    }
}
