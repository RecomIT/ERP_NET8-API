using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.ViewModel.Tax
{
    public class IncomeTaxParameterSettingViewModel : BaseViewModel2
    {
        public long TaxParameterSettingId { get; set; }
        public long FiscalYearId { get; set; }
        [StringLength(20)]
        public string AssessmentYear { get; set; }
        [StringLength(20)]
        public string ImpliedCondition { get; set; }
        public short? MaxTaxAge { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaxInvestmentAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaxInvestmentPercentage { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaxInvesmentExemptedPercentage { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaxAmountForMaxExemptionPercent { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MinInvesmentExemptedPercentage { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MinTaxAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaxHouseRentPercentage { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? HouseRentNotExceding { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaxConveyanceAllowance { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? FreeCar { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? LFAExemptionPercentage { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MedicalExemptionPercentage { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MedicalNotExceding { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaxWPPFExemptionAmount { get; set; }
    }
}
