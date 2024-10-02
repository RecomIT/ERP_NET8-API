using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Payroll.Filter.Incentive.QuarterlyIncentive
{
    public class DownloadExcel_Filter
    {
        public string Quarter { get; set; }
        public string Year { get; set; }
        public string Format { get; set; }
        public string EmployeeIdForSearch { get; set; }
    }
}
