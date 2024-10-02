using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Payroll.Incentive.QuarterlyIncentive.QueryParam
{
    public class QuarterlyIncentiveProcessDetailsUpdate
    {
        public string BankAccountNumber { get; set; }
        public string BankName { get; set; }
        public decimal BankTransferAmount { get; set; }
        public int COCInWalletTransfer { get; set; }
        public int CompanyId { get; set; }
        public decimal CurrentBasic { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string Division { get; set; }
        public decimal EligibleQuarterlyIncentive { get; set; }
        public long? EmployeeId { get; set; }
        public long QuarterlyIncentiveProcessDetailId { get; set; }
        public string IncentiveQuarterNumber { get; set; }
        public short IncentiveYear { get; set; }
        public decimal NetPay { get; set; }
        public int OrganizationId { get; set; }
        public long QuarterlyIncentiveProcessId { get; set; }
        public decimal QuarterlyIncentiveTax { get; set; }
        public decimal TotalDeduction { get; set; }
        public decimal TotalKpiAchievementScore { get; set; }
        public decimal TotalKpiCompanyScore { get; set; }
        public decimal TotalKpiDivisionalIndividualScore { get; set; }
        public decimal TotalQuarterlyIncentive { get; set; }
        public string WalletNumber { get; set; }
        public decimal WalletTransferAmount { get; set; }
    }
}
