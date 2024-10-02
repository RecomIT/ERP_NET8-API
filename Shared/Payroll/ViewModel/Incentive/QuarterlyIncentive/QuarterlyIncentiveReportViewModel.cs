using Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Payroll.ViewModel.Incentive.QuarterlyIncentive
{

    public class QuarterLyIncentive
    {
        public IEnumerable<QuarterlyIncentiveReportViewModel> QuarterlyIncentiveDetails { get; set; }

        // Dynamically add the AmountToWord property to each object in the list
        public void AddAmountToWord()
        {
            foreach (var item in QuarterlyIncentiveDetails)
            {
                item.AmountToWord = NumberToWords.Input(Convert.ToDecimal(item.NetPay));
            }
        }
    }


    public class QuarterlyIncentiveReportViewModel
    {
        public string FirstName { get; set; }
        public string DateOfJoining { get; set; }
        public string OfficeEmail { get; set; }
        public string EmployeeCode { get; set; }
        public string PaymentDate { get; set; }
        public string CompanyName { get; set; }
        public long QuarterlyIncentiveProcessDetailId { get; set; }
        public long QuarterlyIncentiveProcessId { get; set; }
        public string IncentiveQuarterNumber { get; set; }
        public short IncentiveYear { get; set; }
        public long? EmployeeId { get; set; }
        public decimal CurrentBasic { get; set; }
        public decimal TotalKpiCompanyScore { get; set; }
        public decimal TotalKpiDivisionalIndividualScore { get; set; }
        public decimal TotalKpiAchievementScore { get; set; }
        public decimal EligibleQuarterlyIncentive { get; set; }
        public decimal TotalQuarterlyIncentive { get; set; }
        public decimal QuarterlyIncentiveTax { get; set; }
        public decimal TotalDeduction { get; set; }
        public decimal NetPay { get; set; }
        public decimal WalletTransferAmount { get; set; }
        public string WalletNumber { get; set; }
        public decimal COCInWalletTransfer { get; set; }
        public decimal BankTransferAmount { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankName { get; set; }
        public string Division { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public long CompanyId { get; set; }
        public long OrganizationId { get; set; }
        public string AmountToWord { get; set; }
    }
}
