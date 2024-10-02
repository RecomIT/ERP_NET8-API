using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Shared.Payroll.Incentive.QuarterlyIncentive
{
    public class DownloadExcel_Filter
    {
        public string Quarter { get; set; }
        public long Year { get; set; }
        public string Format { get; set; }
        public string EmployeeIdForSearch { get; set; }
    }
}
