using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Payroll.Filter.Incentive.MonthlyIncentive
{
    public class MonthlyIncentiveDetail_Filter
    {
        public string MonthlyIncentiveProcessId { get; set; }
        public string MonthlyIncentiveProcessDetailId { get; set; }
        public string EmployeeIdForSearch { get; set; }
        public string Action { get; set; }
    }
}
