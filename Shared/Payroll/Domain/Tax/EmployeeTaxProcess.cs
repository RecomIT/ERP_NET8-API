using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Tax
{
    [Table("Payroll_EmployeeTaxProcess"), Index("EmployeeId", "FiscalYearId", "SalaryMonth", "SalaryYear", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_EmployeeTaxProcess_NonClusteredIndex")]
    public class EmployeeTaxProcess : BaseModel5
    {
        [Key]
        public long TaxProcessId { get; set; }
        public long EmployeeId { get; set; }
        public long? SalaryProcessId { get; set; }
        public long? SalaryProcessDetailId { get; set; }
        public long FiscalYearId { get; set; }
        public short SalaryMonth { get; set; }
        public short RemainMonth { get; set; }
        public short SalaryYear { get; set; }
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
        public decimal? SupplementaryOnceffTax { get; set; }
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
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CurrentMonthProjectedTaxDeducted { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CurrentMonthOnceOffTaxDeducted { get; set; }
        public string YTDLabel { get; set; }
        public string ProjectionLabel { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TillProjectionTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TillOnceOffTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ActualProjectionTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? AdditionalSubstractAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? DefaultInvestment { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? DefaultRebate { get; set; }
    }
}
