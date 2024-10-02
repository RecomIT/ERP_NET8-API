using Shared.OtherModels.Pagination;

namespace Shared.Payroll.Filter.Payment
{
    public class SupplementaryPaymentProcessInfo_Filter : Sortparam
    {
        public string PaymentMonth { get; set; }
        public string PaymentYear { get; set; }
        public string StateStatus { get; set; }
    }
}
