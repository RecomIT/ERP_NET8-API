using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.DTO.Tax
{
    public class TaxInvestmentSettingDTO
    {
        public long TaxInvestmentSettingId { get; set; }
        public long FiscalYearId { get; set; }
        [StringLength(20)]
        public string ImpliedCondition { get; set; } // Regardleass/MALE/FEMALE/Freedom
        [Required, Column(TypeName = "decimal(18,2)")]
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
        [StringLength(50)]
        public string Flag { get; set; } // Flat/Percentage/Condition
        public long IncomeTaxSettingId { get; set; }
    }
}
