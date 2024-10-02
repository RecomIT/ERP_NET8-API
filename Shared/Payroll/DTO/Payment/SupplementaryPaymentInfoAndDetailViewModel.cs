using Shared.Payroll.ViewModel.Payment;

namespace Shared.Payroll.DTO.Payment
{
    public class SupplementaryPaymentInfoAndDetailViewModel
    {
        public SupplementaryPaymentInfoAndDetailViewModel()
        {
            Info = new SupplementaryPaymentProcessInfoViewModel();
            Details = new List<SupplementaryPaymentAmountViewModel>();
        }
        public SupplementaryPaymentProcessInfoViewModel Info { get; set; }
        public IEnumerable<SupplementaryPaymentAmountViewModel> Details { get; set; }
    }
}
