using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Shared.Helpers.ValidationFilters;

namespace Shared.Payroll.DTO.Payment
{
    public class PeriodicallyVariableAllowanceDTO
    {
        [Key]
        public long Id { get; set; }
        [StringLength(50)]
        public string SpecifyFor { get; set; } // Employee / Grade / Designation / All / Department/ Gender
        [Range(1, long.MaxValue)]
        public long AllowanceNameId { get; set; }
        [Required, StringLength(50)]
        public string AmountBaseOn { get; set; } // Gross / Basic / Flat / Principal
        [Column(TypeName = "decimal(18,2)"), RequiredIfValue("AmountBaseOn", new string[] { "Principal" })]
        public decimal? PrincipalAmount { get; set; }
        [Column(TypeName = "decimal(18,2)"), RequiredIfValue("AmountBaseOn", new string[] { "Principal", "Flat" })]
        public decimal? Amount { get; set; }
        [Column(TypeName = "decimal(18,2)"), RequiredIfValue("AmountBaseOn", new string[] { "Gross", "Basic" })]
        public decimal? Percentage { get; set; }
        [Required,StringLength(50)]
        public string DurationType { get; set; } // Income-Year / Daterange
        [RequiredIfValue("DurationType", new string[] { "Income-Year" })]
        public long? FiscalYearId { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> EffectiveFrom { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> EffectiveTo { get; set; }
        public bool? CalculateProratedAmount { get; set; }
        [StringLength(50)]
        public string JobType { get; set; } // N/A / Permanent / Contrutual / Trainee / Probotion
        [StringLength(50)]
        public string Religion { get; set; }
        [StringLength(50)]
        public string Citizen { get; set; } // N/A / YES /NO
        [StringLength(50)]
        public string Gender { get; set; }
        public bool IsActive { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public bool IsApproved { get; set; }

    }
}
