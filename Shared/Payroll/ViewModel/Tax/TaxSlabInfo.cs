using Shared.Models;
using System.Collections.Generic;

namespace Shared.Payroll.ViewModel.Tax
{
    public class TaxSlabInfo : BaseViewModel1
    {
        public ICollection<TaxSlabDetail> TaxSlabDetails { get; set; }
    }
}
