using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Tools.ViewModel
{
    public class ReportTaxCardIncomeHead
    {
        public string EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public string IncomeAnnualGrossName { get; set; }

        public decimal IncomeTillDateAmount { get; set; }

        public decimal IncomeCurrentMonthAmount { get; set; }

        public decimal IncomeProjectedDateAmount { get; set; }

        public decimal IncomeAnnualGrossAmount { get; set; }

        public decimal ExemptedLessAmount { get; set; }

        public decimal IncomeTotalTaxableAmount { get; set; }

        public string TillDateRange { get; set; }

        public string ProjectedDateRange { get; set; }
    }
}
