using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.DTO.Variable
{
    public class PeriodicalAllowanceInfoDTO
    {
        public long Id { get; set; }
        [StringLength(50,ErrorMessage = "The maximum lenght of specify for is 50"),Required(ErrorMessage = "Please mention specify for")]
        public string SpecifyFor { get; set; } // Employee / Grade / Designation / All / Department/ Gender
        [StringLength(50, ErrorMessage = "The maximum lenght of amount base-on is 50"), Required(ErrorMessage = "Amount base on is missing")]
        public string AmountBaseOn { get; set; } // Gross / Basic / Flat / Principal
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PrincipalAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Percentage { get; set; }
        [StringLength(50)]
        public string DurationType { get; set; } // Income-Year / Daterange
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
        public long AllowanceNameId { get; set; }
        public List<PeriodicalAllowanceDetailDTO> Details { get; set; }
        public List<PrincipleAmountInfoDTO> PrincipleAmounts { get; set; }
}
}
