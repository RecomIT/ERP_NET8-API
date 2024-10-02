using Shared.OtherModels.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Payroll.Filter.Incentive.QuarterlyIncentive
{
    public class QuarterlyIncentive_Filter : Sortparam
    {
        public string BatchNo { get; set; }
        public string IncentiveQuarterNoId { get; set; }
        public string IncentiveQuarterNumber { get; set; }
        public string IncentiveYear { get; set; }
        public string IsDisbursed { get; set; }

    }
}
