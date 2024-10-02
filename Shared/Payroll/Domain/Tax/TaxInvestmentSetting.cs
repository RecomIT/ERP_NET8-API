using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Tax
{
    [Table("Payroll_TaxInvestmentSetting"), Index("FiscalYearId", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_TaxInvestmentSetting_NonClusteredIndex")]
    public class TaxInvestmentSetting : BaseModel2
    {
        [Key]
        public long TaxInvestmentSettingId { get; set; }
        public long FiscalYearId { get; set; }
        [StringLength(20)]
        public string ImpliedCondition { get; set; } // Regardleass/MALE/FEMALE/Freedom
        [Column(TypeName = "decimal(18,2)")]
        public decimal MaxInvestmentPercentage { get; set; } // Annual Income Inv Per.
        [Column(TypeName = "decimal(18,2)")]
        public decimal? RebateAmount { get; set; } // 1500000
        [StringLength(20)]
        public string Operator { get; set; } // <,>,<=,>=,= 1500000 > 10 OR 15
        [Column(TypeName = "decimal(4,2)")]
        public decimal MinRebate { get; set; }
        // OR
        [Column(TypeName = "decimal(4,2)")]
        public decimal MaxRebate { get; set; }
        public bool IsFlatRebate { get; set; }
        public string Flag { get; set; } // Flat/Percentage/Condition
        public long IncomeTaxSettingId { get; set; }
    }
}
