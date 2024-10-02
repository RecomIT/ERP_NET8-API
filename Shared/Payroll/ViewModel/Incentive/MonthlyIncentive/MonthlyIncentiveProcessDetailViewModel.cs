using Shared.BaseModels.For_ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Payroll.ViewModel.Incentive.MonthlyIncentive
{
    public class MonthlyIncentiveProcessDetailViewModel : BaseViewModel1
    {
        public long? MonthlyIncentiveProcessDetailId { get; set; }
        public long? MonthlyIncentiveProcessId { get; set; }
        [StringLength(50)]
        public string EmployeeCode { get; set; }
        [StringLength(150)]
        public string EmployeeName { get; set; }
        [StringLength(100)]
        public string BatchNo { get; set; }
        public short IncentiveMonth { get; set; }
        public short IncentiveYear { get; set; }
        public long? MonthlyIncentiveNoId { get; set; }
        [StringLength(100)]
        public string MonthlyIncentiveName { get; set; }
        [Column(TypeName = "decimal(18,6)")]
        public decimal? AttendanceScore { get; set; }
        public long? EmployeeId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CurrentBasic { get; set; }
        [Column(TypeName = "decimal(18,6)")]
        public decimal? AdjustedKpiPerformanceScore { get; set; }
        [StringLength(50)]
        public string ESSAURating { get; set; }
        [Column(TypeName = "decimal(18,6)")]
        public decimal? AttendanceAdherenceQualityScore { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? EligibleIncentive { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalIncentive { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Adjustment { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalAdjustment { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? IncentiveTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalDeduction { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? NetPay { get; set; }
        [StringLength(50)]
        public string PaymentMode { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? WalletTransferAmount { get; set; }
        public bool? WithCOC { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? COCInWalletTransfer { get; set; }
        [StringLength(100)]
        public string WalletNumber { get; set; }
        [StringLength(50)]
        public string WalletAgent { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? BankTransferAmount { get; set; }
        [StringLength(100)]
        public string BankAccountNumber { get; set; }
        [StringLength(100)]
        public string BankName { get; set; }
        [StringLength(100)]
        public string BankBranchName { get; set; }
        [StringLength(150)]
        public string RoutingNumber { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CashAmount { get; set; }
        [StringLength(100)]
        public string PayrollCardNumber { get; set; }
        [StringLength(100)]
        public string Division { get; set; }
        [StringLength(100)]
        public string Department { get; set; }
        [StringLength(100)]
        public string Designation { get; set; }
        [StringLength(50)]
        public string Grade { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        [StringLength(50)]
        public string IncentiveMonthName { get; set; }
        [Column(TypeName = "date")]
        public DateTime? JoiningDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? PaymentDate { get; set; }
    }
}
