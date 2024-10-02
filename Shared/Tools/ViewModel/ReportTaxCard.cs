using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Tools.ViewModel
{
    public class ReportTaxCard
    {
        public string EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public string Designation { get; set; }

        public string Gender { get; set; }

        public string FiscalYear { get; set; }

        public string AssesmentYear { get; set; }

        public string TIN { get; set; }

        public decimal TotalAnnualIncomeGross { get; set; }

        public decimal TotalExemptionOfAnnualIncome { get; set; }

        public decimal TotalTaxableIncome { get; set; }

        public decimal TaxLiability { get; set; }

        public decimal PFContributionBothParts { get; set; }

        public decimal OtherInvestmentExceptPF { get; set; }

        public decimal ActualInvestementTotal { get; set; }

        public decimal NetRebateAmount { get; set; }

        public decimal AdvanceIncomeTax { get; set; }

        public decimal RefundAmount { get; set; }

        public decimal NetTaxPayable { get; set; }

    }
}
