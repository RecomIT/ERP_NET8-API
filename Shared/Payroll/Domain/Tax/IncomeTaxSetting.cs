using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Tax
{
    [Table("Payroll_IncomeTaxSetting"), Index("FiscalYearId", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_IncomeTaxSetting_NonClusteredIndex")]
    public class IncomeTaxSetting : BaseModel
    {
        [Key]
        public long IncomeTaxSettingId { get; set; }
        public long FiscalYearId { get; set; }
        [StringLength(20)]
        public string AssessmentYear { get; set; }
        [StringLength(20)]
        public string ImpliedCondition { get; set; }
        public short? MaxTaxAge { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MinTaxAmount { get; set; }
        public bool IsFlatRebate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ExemptionAmountOfAnnualIncome { get; set; } // BD: FY 22-23
        [Column(TypeName = "decimal(18,11)")]
        public decimal? ExemptionPercentageOfAnnualIncome { get; set; } // BD: FY 22-23
        public int? FreeCarCCMinimumLimit { get; set; } // BD: FY 22-23
        [Column(TypeName = "decimal(18,2)")]
        public decimal? FreeCarMinTaxableAmount { get; set; } // BD: FY 22-23
        [Column(TypeName = "decimal(18,2)")]
        public decimal? FreeCarMaxTaxableAmount { get; set; } // BD: FY 22-23
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonthlyTaxDeductionPercentage { get; set; }
        public bool? CalculateTaxOnArrearAmount { get; set; }
        public bool? PFBothPartExemption { get; set; }
        public short NonResidentialPeriod { get; set; } = 0;
    }
}
