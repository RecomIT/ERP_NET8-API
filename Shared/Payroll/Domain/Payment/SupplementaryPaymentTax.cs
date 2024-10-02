using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Payment
{
    [Table("Payroll_SupplementaryPaymentTaxInfo"), Index("EmployeeId", "FiscalYearId", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_SupplementaryPaymentTaxInfo_NonClusteredIndex")]
    public class SupplementaryPaymentTaxInfo : BaseModel1
    {
        [Key]
        public long PaymentTaxInfoId { get; set; }
        public long EmployeeId { get; set; }
        public long? PaymentProcessInfoId { get; set; }
        public long? PaymentAmountId { get; set; }
        public long FiscalYearId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal RemainMonth { get; set; }
        public short PaymentMonth { get; set; }
        public short PaymentYear { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalTillMonthAllowanceAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalCurrentMonthAllowanceAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalProjectedAllowanceAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalAdjustmentAmount { get; set; } // Adjustment
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalLessExemptedAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalGrossAnnualIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? GrossTaxableIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ExemptionAmountOnAnnualIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PFContributionBothPart { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? OtherInvestment { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TaxableIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? YearlyTaxableIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalTaxPayable { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? InvestmentRebateAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ActualInvestmentMade { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? AITAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TaxReturnAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ExcessTaxPaidRefundAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? YearlyTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PaidTotalTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal MonthlyTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ProjectionTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? OnceOffTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ProjectionAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? OnceOffAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ArrearAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PFExemption { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalIncomeAfterPFExemption { get; set; }
        [StringLength(100)]
        public string TaxProcessUniqId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ActualTaxDeductionAmount { get; set; }
        public short? PayableMonth { get; set; }
        public short? PayableYear { get; set; }
    }
}
