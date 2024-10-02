using Shared.OtherModels.Pagination;

namespace Shared.Payroll.Filter.Salary
{
    public class SalaryReview_Filter : Sortparam
    {
        public string SalaryReviewInfoId { get; set; }
        public string EmployeeId { get; set; }
        public string StateStatus { get; set; }
        public string SalaryConfigCategory { get; set; }
    }
}
