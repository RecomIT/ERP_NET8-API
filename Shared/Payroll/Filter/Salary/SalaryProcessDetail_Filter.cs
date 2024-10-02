using Shared.OtherModels.Pagination;

namespace Shared.Payroll.Filter.Salary
{
    public class SalaryProcessDetail_Filter : Sortparam
    {
        public string SalaryProcessId { get; set; }
        public string SalaryProcessDetailId { get; set; }
        public string EmployeeId { get; set; }
        public string FiscalYearId { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string BranchId { get; set; }
        public string BatchNo { get; set; }
    }
}
