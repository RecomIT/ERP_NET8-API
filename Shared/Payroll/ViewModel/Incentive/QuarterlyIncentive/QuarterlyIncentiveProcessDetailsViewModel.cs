using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Payroll.ViewModel.Incentive.QuarterlyIncentive
{
    public class QuarterlyIncentiveProcessDetailsViewModel : BaseViewModel2
    {
        public long QuarterlyIncentiveProcessId { get; set; }
        public long QuarterlyIncentiveProcessDetailId { get; set; }
        [StringLength(50)]
        public string EmployeeCode { get; set; }
        [StringLength(150)]
        public string EmployeeName { get; set; }
        public long? EmployeeId { get; set; }
        [StringLength(100)]
        public string Division { get; set; }
        [StringLength(100)]
        public string Designation { get; set; }
        [StringLength(100)]
        public string Grade { get; set; }
        [StringLength(100)]
        public string Department { get; set; }
        public short IncentiveYear { get; set; }
        public long? IncentiveQuarterNoId { get; set; }
        [StringLength(100)]
        public string IncentiveQuarterNumber { get; set; }
        [StringLength(100)]
        public string BatchNo { get; set; }
        [Column(TypeName = "date")]
        public DateTime? PaymentDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CurrentBasic { get; set; }
        [Column(TypeName = "decimal(18,6)")]
        public decimal? TotalKpiCompanyScore { get; set; }
        [Column(TypeName = "decimal(18,6)")]
        public decimal? TotalKpiDivisionalIndividualScore { get; set; }
        [Column(TypeName = "decimal(18,6)")]
        public decimal? TotalKpiAchievementScore { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? EligibleQuarterlyIncentive { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalQuarterlyIncentive { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? QuarterlyIncentiveTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalDeduction { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? NetPay { get; set; }
        [StringLength(50)]
        public string PaymentMode { get; set; }
        [StringLength(150)]
        public string BankName { get; set; }
        [StringLength(150)]
        public string BankBranchName { get; set; }
        [StringLength(150)]
        public string RoutingNumber { get; set; }
        [StringLength(100)]
        public string BankAccountNumber { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? BankTransferAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? WalletTransferAmount { get; set; }
        public bool? WithCOC { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? COCInWalletTransfer { get; set; }
        [StringLength(50)]
        public string WalletNumber { get; set; }
        [StringLength(50)]
        public string WalletAgent { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CashAmount { get; set; }
        [StringLength(100)]
        public string PayrollCardNumber { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        [Column(TypeName = "decimal(18,6)")]
        public decimal? kpiScore { get; set; }
        [Column(TypeName = "decimal(18,6)")]
        public decimal? DivisionalAndIndividualScore { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DOJ { get; set; }
        [StringLength(50)]
        public string OfficeEmail { get; set; }
        public bool? IsDisbursed { get; set; }
        public bool? IsApproved { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
    }
}
