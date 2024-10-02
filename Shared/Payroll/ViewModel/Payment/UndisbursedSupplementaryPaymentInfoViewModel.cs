

using iText.Layout.Element;
using System.Collections.Generic;

namespace Shared.Payroll.ViewModel.Payment
{
    public class UndisbursedSupplementaryPaymentInfoViewModel
    {
        public UndisbursedSupplementaryPaymentInfoViewModel()
        {
            Info = new SupplementaryPaymentProcessInfoViewModel();
            Details = new List<SupplementaryPaymentAmountViewModel>();
        }
        public SupplementaryPaymentProcessInfoViewModel Info { get; set; }
        public IEnumerable<SupplementaryPaymentAmountViewModel> Details { get; set; }
    }
}
