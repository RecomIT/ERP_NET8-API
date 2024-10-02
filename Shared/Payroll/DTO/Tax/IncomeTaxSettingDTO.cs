using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.DTO.Tax
{
    public class IncomeTaxSettingDTO
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
        public decimal? MinTaxAmount { get; set; }
        //[Column(TypeName = "decimal(18,2)")]
        //public decimal? RebateAmount { get; set; }
        public bool IsFlatRebate { get; set; }
        [Column(TypeName ="decimal(18,2)")]
        public decimal? ExemptionAmountOfAnnualIncome { get; set; }
        [Column(TypeName = "decimal(18,11)")]
        public decimal? ExemptionPercentageOfAnnualIncome { get; set; }
        public int? FreeCarCCMinimumLimit { get; set; } // BD: FY 22-23
        [Column(TypeName = "decimal(18,2)")]
        public decimal? FreeCarMinTaxableAmount { get; set; } // BD: FY 22-23
        [Column(TypeName = "decimal(18,2)")]
        public decimal? FreeCarMaxTaxableAmount { get; set; } // BD: FY 22-23
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonthlyTaxDeductionPercentage { get; set; }
        public bool? CalculateTaxOnArrearAmount { get; set; }
        public bool? PFBothPartExemption { get; set; }
    }
}
