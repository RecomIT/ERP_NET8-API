using Shared.OtherModels.Pagination;

namespace Shared.Payroll.Filter.Payment
{
    public class EmployeeProjectedPayment_Filter : Sortparam
    {
        public string ProjectedAllowanceId { get; set; }
        public string ProjectedAllowanceCode { get; set; }
        public string AllowanceNameId { get; set; }
        public string FiscalYearId { get; set; }
        public string EmployeeId { get; set; }
        public string Reason { get; set; }
        public string PaymentMonth { get; set; }
        public string PaymentYear { get; set; }
        public string PayableYear { get; set; }
        public string StateStatus { get; set; }
    }
}
