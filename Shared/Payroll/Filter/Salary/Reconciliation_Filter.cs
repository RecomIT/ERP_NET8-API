using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Payroll.Filter.Salary
{
    public class Reconciliation_Filter
    {
        public string SalaryMonth { get; set; }
        public string SalaryYear { get; set; }
        public string Format { get; set; }
        public string BranchId { get; set; }
    }
}
