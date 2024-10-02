using System;
using Shared.Models;
using Shared.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Helpers.ValidationFilters;

namespace Shared.Payroll.ViewModel.Bonus
{
    public class BonusConfigViewModel : BaseViewModel4
    {
        public long BonusConfigId { get; set; }
        [StringLength(50)]
        public string BonusConfigCode { get; set; }
        [Range(1, long.MaxValue)]
        public long FiscalYearId { get; set; }
        [Required]
        public bool? IsConfirmedEmployee { get; set; }
        [Required]
        public bool IsReligious { get; set; }
        [StringLength(100)]
        public string ReligionName { get; set; }
        [Required]
        public bool? IsFestival { get; set; }
        [Required]
        public bool? IsTaxable { get; set; }
        public bool? IsPaymentProjected { get; set; }
        public bool? IsTaxDistributed { get; set; }
        [Required]
        public bool? IsOnceOff { get; set; }
        public bool? IsExcludeFromSalary { get; set; } // If IsOnceOff true then by IsExcludeSalary is false
        public bool? IsTaxAdjustedWithSalary { get; set; } //  If IsExcludeSalary is true then IsTaxAdjustedWithSalary is false by default
        public bool? IsPerquisite { get; set; }
        [Required, StringLength(50)]
        public string BasedOn { get; set; }
        // GROSS // BASIC // FLAT
        [RequiredAmountOrPercentage("Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Percentage { get; set; }
        [Range(1, short.MaxValue)]
        public short BonusCount { get; set; }
        [RequiredAmountOrPercentage("Percentage")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaximumAmount { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        [Range(1, long.MaxValue)]
        public long BonusId { get; set; }
        public string BonusName { get; set; }
        public string FiscalYearRange { get; set; }

    }
}
