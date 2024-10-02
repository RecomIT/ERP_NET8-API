using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Process.Tax
{
    public class TaxDetailInTaxProcess
    {
        public int SL { get; set; }
        public long EmployeeId { get; set; }
        public long AllowanceHeadId { get; set; }
        public string AllowanceHeadName { get; set; }
        public long AllowanceNameId { get; set; }
        public string AllowanceName { get; set; }
        public string TaxItem { get; set; }
        public long? DeductionNameId { get; set; }
        public string DeductionName { get; set; }
        public short Month { get; set; }
        public short Year { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsMonthly { get; set; }
        public bool? IsTaxable { get; set; }
        public bool? IsIndividual { get; set; }
        public bool? DepandsOnWorkingHour { get; set; }
        public bool? ProjectRestYear { get; set; }
        public bool? OnceOffDeduction { get; set; }
        public long AllowanceConfigId { get; set; }
        [StringLength(100)]
        public string Flag { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TillAmount { get; set; }
        public decimal TillAdjustment { get; set; } = 0;
        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentAmount { get; set; }
        public decimal CurrentAdjustment { get; set; } = 0;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Arrear { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ReviewAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Adjustment { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ProjectedAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal GrossAnnualIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal LessExemptedAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalTaxableIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ExemptionPercentage { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ExemptionAmount { get; set; }
        public short RemainFiscalYearMonth { get; set; }
        public bool IsItAllowance { get; set; } = true;

    }
}
