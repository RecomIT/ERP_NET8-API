using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Payroll.ViewModel.Tax
{
    public class IncomeTaxSettingViewModel : BaseViewModel2
    {
        public long IncomeTaxSettingId { get; set; }
        [Range(1, long.MaxValue)]
        public long FiscalYearId { get; set; }
        [StringLength(20)]
        public string AssessmentYear { get; set; }
        [Required, StringLength(20)]
        public string ImpliedCondition { get; set; }
        public short? MaxTaxAge { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal MinTaxAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ExemptionAmountOfAnnualIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ExemptionPercentageOfAnnualIncome { get; set; }
        public int? FreeCarCCMinimumLimit { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? FreeCarMinTaxableAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? FreeCarMaxTaxableAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonthlyTaxDeductionPercentage { get; set; }
        public bool? CalculateTaxOnArrearAmount { get; set; }
        public bool? PFBothPartExemption { get; set; }
        public bool IsFlatRebate { get; set; }
        public string FiscalYearRange { get; set; }

    }
}
