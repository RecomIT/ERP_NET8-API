using Shared.OtherModels.Pagination;

namespace Shared.Payroll.Filter.Payment
{
    public class MonthlyAllowanceConfig_Filter : Sortparam
    {
        public string AllowanceNameId { get; set; }
        public string EmployeeId { get; set; }
        public string StateStatus { get; set; }
    }
}
