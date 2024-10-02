using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.DTO.Allowance
{
    public class AllowanceConfigurationDTO
    {
        public long ConfigId { get; set; }
        [Required]
        public long AllowanceNameId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsMonthly { get; set; }
        public bool? IsTaxable { get; set; }
        public bool? IsIndividual { get; set; }
        [StringLength(20)]
        public string Gender { get; set; }
        public bool? IsConfirmationRequired { get; set; }
        public bool? DepandsOnWorkingHour { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> ActivationDate { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> DeactivationDate { get; set; }
        public bool? IsPerquisite { get; set; }
        [StringLength(100)]
        public string TaxConditionType { get; set; }
        public bool? ProjectRestYear { get; set; } // IS PAYMENT PROJECTED
        public bool? IsTaxDistributed { get; set; } // IS TAX Distributed
        [Column(TypeName = "decimal(18,2)")]
        public decimal? OnceOffDeduction { get; set; }
        public bool? IsOnceOffTax { get; set; } // IS Once Off
        [Column(TypeName = "decimal(18,2)")]
        public decimal? FlatAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PercentageAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaxAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MinAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ExemptedAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ExemptedPercentage { get; set; }
        public bool IsApproved { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
