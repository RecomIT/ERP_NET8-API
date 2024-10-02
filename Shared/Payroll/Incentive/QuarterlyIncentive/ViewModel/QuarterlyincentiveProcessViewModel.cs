using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Payroll.Incentive.QuarterlyIncentive.ViewModel
{
    public class QuarterlyincentiveProcessViewModel
    {
        public string BatchNo { get; set; }
        public string IncentiveQuarterNumber { get; set; }
        public short IncentiveYear { get; set; }
        public DateTime? PaymentDate { get; set; }
    }
}
