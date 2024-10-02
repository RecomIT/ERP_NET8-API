using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.ViewModel.Tax
{
    public class TaxExemptionSettingViewModel : BaseViewModel2
    {
        public long TaxExemptionSettingId { get; set; }
        public long FiscalYearId { get; set; }
        [StringLength(20)]
        public string ImpliedCondition { get; set; }
        [Required, StringLength(100)]
        public string Allowance { get; set; } //HR/LFA/MED/CAR/WPPR/CONVEYANCE
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaxExemptionPercentage { get; set; }
        [StringLength(100)]
        public string BasedOfAllowance { get; set; } //Annual Basic/LFA/(N/A)
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaxExemptionAmount { get; set; }
        public bool? TakeLowerAmount { get; set; }
        public bool? TakeMinAmount { get; set; }
        public bool? UptoMaxAmount { get; set; }
        public bool? UptoMaxPercentage { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public string ExemptionRemarks { get; set; }
        public long IncomeTaxSettingId { get; set; }
    }
}
