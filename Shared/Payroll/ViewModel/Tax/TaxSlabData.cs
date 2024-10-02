using System.Collections.Generic;

namespace Shared.Payroll.ViewModel.Tax
{
    public class TaxSlabData
    {
        public long FiscalYearId { get; set; }
        public string FiscalYearRange { get; set; }
        public string AssesmentYear { get; set; }
        public string ImpliedCondition { get; set; }
        public List<TaxSlabAmount> TaxSlabAmounts { get; set; }
    }
}
