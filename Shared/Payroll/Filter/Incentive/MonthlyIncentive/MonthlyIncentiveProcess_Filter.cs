using Shared.OtherModels.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Payroll.Filter.Incentive.MonthlyIncentive
{
    public class MonthlyIncentiveProcess_Filter : Sortparam
    {
        public string BatchNo { get; set; }
        public string MonthlyIncentiveNoId { get; set; }
        public string MonthlyIncentiveName { get; set; }
        public string IncentiveYear { get; set; }
        public string IncentiveMonth { get; set; }
        public string IsDisbursed { get; set; }
        public string StateStatus { get; set; }
    }
}
