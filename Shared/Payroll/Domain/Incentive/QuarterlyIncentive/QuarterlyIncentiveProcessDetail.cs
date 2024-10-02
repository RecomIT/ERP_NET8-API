using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Incentive.QuarterlyIncentive
{

    [Table("Payroll_QuarterlyIncentiveProcessDetail")]
    public class QuarterlyIncentiveProcessDetail : BaseModel3
    {
        [Key]
        public long QuarterlyIncentiveProcessDetailId { get; set; }
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

        [StringLength(100)]
        public string PayableHeadName { get; set; }
        public short? PayableMonth { get; set; }
        public short? PayableYear { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Adjustment { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalAdjustment { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> PayableDate { get; set; }
        [ForeignKey("QuarterlyIncentiveProcess")]
        public long QuarterlyIncentiveProcessId { get; set; }
        public QuarterlyIncentiveProcess QuarterlyIncentiveProcess { get; set; }
    }
}


