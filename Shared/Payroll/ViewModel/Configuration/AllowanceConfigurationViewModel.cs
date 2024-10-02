using System;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Payroll.ViewModel.Configuration
{
    public class AllowanceConfigurationViewModel : BaseViewModel3
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
        public DateTime? ActivationDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DeactivationDate { get; set; }
        public bool? ProjectRestYear { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? OnceOffDeduction { get; set; }
        public bool? IsOnceOffTax { get; set; }
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

        // Custom Property
        [StringLength(100)]
        public string AllowanceName { get; set; }
        [StringLength(100)]
        public string AllowanceFlag { get; set; }
    }
}
