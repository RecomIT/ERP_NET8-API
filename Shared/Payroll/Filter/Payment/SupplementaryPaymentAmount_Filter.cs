using Shared.OtherModels.Pagination;

namespace Shared.Payroll.Filter.Payment
{
    public class SupplementaryPaymentAmount_Filter : Sortparam
    {
        public string PaymentAmountId { get; set; }
        public string PaymentMonth { get; set; }
        public string PaymentYear { get; set; }
        public string EmployeeId { get; set; }
        public string AllowanceNameId { get; set; }
        public string Status { get; set; }
    }
}
