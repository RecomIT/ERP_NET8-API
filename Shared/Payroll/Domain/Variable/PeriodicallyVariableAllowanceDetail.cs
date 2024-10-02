using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Variable
{
    [Table("Payroll_PeriodicallyVariableAllowanceDetail"), Index("AllowanceNameId", "SpecifyFor", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_AllowanceHead_NonClusteredIndex")]
    public class PeriodicallyVariableAllowanceDetail : BaseModel1
    {
        [Key]
        public long Id { get; set; }
        public long AllowanceNameId { get; set; }
        [StringLength(50)]
        public string SpecifyFor { get; set; } // Employee / Grade / Designation / All
        public long? EmployeeId { get; set; }
        public long? DesignationId { get; set; }
        public long? GradeId { get; set; }
        public long? DepartmentId { get; set; }
        [StringLength(50)]
        public string JobType { get; set; }
        //// Duration
        [StringLength(50)]
        public string DurationType { get; set; } // IncomeYear / DateRange
        public long? FiscalYearId { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EffectiveFrom { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EffectiveTo { get; set; }
        //// Amount
        [StringLength(50)]
        public string AmountBaseOn { get; set; } // Gross / Basic / Flat / Principal
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PrincipalAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Percentage { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }
        public bool? IsActive { get; set; }
        public Nullable<DateTime> InActiveDate { get; set; }
        public bool? CalculatePoratedAmount { get; set; }
        [ForeignKey("PeriodicallyVariableAllowanceInfo")]
        public long InfoId { get; set; }
        public PeriodicallyVariableAllowanceInfo PeriodicallyVariableAllowanceInfo { get; set; }

    }
}
