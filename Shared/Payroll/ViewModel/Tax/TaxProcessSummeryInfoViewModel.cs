using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.ViewModel.Tax
{
    public class TaxProcessSummeryInfoViewModel : BaseViewModel2
    {
        public long TaxProcessId { get; set; }
        public long? HeadCount { get; set; }
        public long? FiscalYearId { get; set; }
        public short? SalaryMonth { get; set; }
        public short? SalaryYear { get; set; }
        [StringLength(100)]
        public string SalaryMonthYear { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? YearlyTaxableIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalTaxPayable { get; set; }
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
        public decimal? ProjectionTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ProjectionAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? OnceOffTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? OnceOffAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonthlyTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PFContributionBothPart { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? OtherInvestment { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ActualInvestmentMade { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? InvestmentRebateAmount { get; set; }
        [StringLength(100)]
        public string FiscalYearRange { get; set; }
        [StringLength(100)]
        public string AssesmentYear { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ActualTaxDeductionAmount { get; set; }
        public long SalaryProcessId { get; set; }
        public string BatchNo { get; set; }
        public bool IsDisbursed { get; set; }
    }
}
