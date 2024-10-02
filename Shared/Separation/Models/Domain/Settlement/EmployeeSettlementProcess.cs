using Shared.BaseModels.For_DomainModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Separation.Models.Domain.Settlement
{
    [Table("Payroll_EmployeeSettlementProcess")]
    public class EmployeeSettlementProcess : BaseModel
    {
        [Key]
        public long SettlementProcessId { get; set; }
        public long? SettlementSetupId { get; set; }
        public DateTime? SettlementDate { get; set; }
        public long? ResignationRequestId { get; set; }
        public long? EmployeeId { get; set; }
        public string ClearanceStatus { get; set; }
        public string ClearanceBy { get; set; }
        public bool? IsLeaveEncashment { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? LeaveEncashmentAmount { get; set; }
        public bool? IsProvidentFundFinalization { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PfOwnAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PfCompanyAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PfOwnDividendAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PfCompanyDividendAmount { get; set; }
        public bool? IsGFFinalization { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? GFFinalizationAmount { get; set; }
        public bool? IsWPPFFinalization { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? WPPFFinalizationAmount { get; set; }
        public bool? IsRefund { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? RefundAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? FractionalSalaryAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? BonusEarningAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? OTAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? OtherAllowancesAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalEmployeeEarnings { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? FractionalSalaryDeduction { get; set; }
        public bool? IsBonusDeduction { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? BonusDeduction { get; set; }
        public bool? IsLoanAmountDeduction { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? LoanDeductionAmount { get; set; }
        public bool? IsOtherDeductions { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? OtherDeductions { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalAmount { get; set; }
    }
}
