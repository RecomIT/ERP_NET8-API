using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Variable
{
    [Table("Payroll_PeriodicallyVariableAllowanceInfo"), Index("SpecifyFor", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_AllowanceHeadPayroll_PeriodicallyVariableAllowanceInfo_NonClusteredIndex")]
    public class PeriodicallyVariableAllowanceInfo : BaseModel2
    {
        [Key]
        public long Id { get; set; }
        [StringLength(50)]
        public string SpecifyFor { get; set; } // Employee / Grade / Designation / All / Department/ Gender
        [StringLength(50)]
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
        public ICollection<PeriodicallyVariableAllowanceDetail> PeriodicallyVariableAllowanceDetails { get; set; }
    }
}
