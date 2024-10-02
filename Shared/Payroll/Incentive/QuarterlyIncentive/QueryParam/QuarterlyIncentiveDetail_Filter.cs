using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Payroll.Incentive.QuarterlyIncentive.QueryParam
{
    public class QuarterlyIncentiveDetail_Filter
    {
        public string QuarterlyIncentiveProcessId { get; set; }
        public string QuarterlyIncentiveProcessDetailId { get; set; }
        public string EmployeeIdForSearch { get; set; }
        public string Action { get; set; }
    }
}
