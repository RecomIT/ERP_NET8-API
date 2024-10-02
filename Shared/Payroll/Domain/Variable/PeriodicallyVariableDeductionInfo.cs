using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Shared.Payroll.Domain.Deduction;

namespace Shared.Payroll.Domain.Variable
{
    [Table("Payroll_PeriodicallyVariableDeductionInfo"), Index("SalaryVariableFor", "AmountBaseOn", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_PeriodicallyVariableDeductionInfoeHead_NonClusteredIndex")]
    public class PeriodicallyVariableDeductionInfo : BaseModel3
    {
        [Key]
        public long PeriodicallyVariableDeductionInfoId { get; set; }
        [StringLength(50)]
        public string SalaryVariableFor { get; set; } // Employee / Grade / Designation / All
        [StringLength(50)]
        public string AmountBaseOn { get; set; } // Gross / Basic / Flat / Principal
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PrincipalAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Percentage { get; set; }
        [StringLength(50)]
        public string DurationType { get; set; } // IncomeYear / DateRange
        public long? FiscalYearId { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EffectiveFrom { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EffectiveTo { get; set; }
        public bool IsActive { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public bool IsApproved { get; set; }
        [ForeignKey("DeductionName")]
        public long DeductionNameId { get; set; }
        public DeductionName DeductionName { get; set; }
        public ICollection<PeriodicallyVariableDeductionDetail> PeriodicallyVariableDeductionDetails { get; set; }
    }
}
