using Shared.OtherModels.Pagination;
namespace Shared.Payroll.Filter.Salary
{
    public class SalaryAllowanceArrearAdjustment_Filter : Sortparam
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string AllowanceNameId { get; set; }
        public string Flag { get; set; }
        public string SalaryYear { get; set; }
        public string SalaryMonth { get; set; }
        public string StateStatus { get; set; }
    }
}
