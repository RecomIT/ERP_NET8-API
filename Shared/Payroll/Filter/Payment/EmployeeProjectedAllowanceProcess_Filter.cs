using Shared.OtherModels.Pagination;

namespace Shared.Payroll.Filter.Payment
{
    public class EmployeeProjectedAllowanceProcess_Filter : Sortparam
    {
        public string FiscalYearId { get; set; }
        public string StateStatus { get; set; }
        public string PaymentMonth { get; set; }
        public string PaymentYear { get; set; }
    }
}
