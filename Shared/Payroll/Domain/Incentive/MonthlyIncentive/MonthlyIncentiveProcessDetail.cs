using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Incentive.MonthlyIncentive
{
    [Table("Payroll_MonthlyIncentiveProcessDetail")]
    public class MonthlyIncentiveProcessDetail : BaseModel2
    {
        [Key]
        public long MonthlyIncentiveProcessDetailId { get; set; }
        [ForeignKey("MonthlyIncentiveProcess")]
        public long MonthlyIncentiveProcessId { get; set; }
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
        [StringLength(100)]
        public string PayableHeadName { get; set; }
        public short? PayableMonth { get; set; }
        public short? PayableYear { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> PayableDate { get; set; }
        public MonthlyIncentiveProcess MonthlyIncentiveProcess { get; set; }
    }
}
