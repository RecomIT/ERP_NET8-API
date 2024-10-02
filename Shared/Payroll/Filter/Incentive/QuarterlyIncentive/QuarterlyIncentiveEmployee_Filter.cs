using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Payroll.Filter.Incentive.QuarterlyIncentive
{
    public class QuarterlyIncentiveEmployee_Filter
    {
        public string IncentiveYear { get; set; }
        public string IncentiveQuarterNumber { get; set; }
        public string BatchNo { get; set; }
        public string IsDisbursed { get; set; }

    }
}
