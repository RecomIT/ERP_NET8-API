using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Payroll.Filter.Incentive.MonthlyIncentive
{
    public class UpdateMonthlyIncentiveProcessDetails_Filter
    {
        public long MonthlyIncentiveProcessId { get; set; }
        public long MonthlyIncentiveProcessDetailId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankName { get; set; }
        public decimal BankTransferAmount { get; set; }
        public decimal COCInWalletTransfer { get; set; }
        public long CompanyId { get; set; }
        public decimal CurrentBasic { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string Division { get; set; }
        public decimal EligibleIncentive { get; set; }
        public long? EmployeeId { get; set; }
        public long? MonthlyIncentiveNoId { get; set; }
        public string IncentiveMonthName { get; set; }
        public string MonthlyIncentiveName { get; set; }
        public short IncentiveYear { get; set; }
        public short IncentiveMonth { get; set; }
        public decimal NetPay { get; set; }
        public long OrganizationId { get; set; }
        public decimal IncentiveTax { get; set; }
        public decimal TotalDeduction { get; set; }
        public decimal AdjustedKpiPerformanceScore { get; set; }
        public string ESSAURating { get; set; }
        public decimal AttendanceAdherenceQualityScore { get; set; }
        public decimal TotalIncentive { get; set; }
        public string WalletNumber { get; set; }
        public decimal WalletTransferAmount { get; set; }
    }
}
