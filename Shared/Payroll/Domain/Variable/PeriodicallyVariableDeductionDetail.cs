using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Variable
{
    [Table("Payroll_PeriodicallyVariableDeductionDetail")]
    public class PeriodicallyVariableDeductionDetail : BaseModel1
    {
        [Key]
        public long PeriodicallyVariableDeductionDetailId { get; set; }
        public long DeductionNameId { get; set; }
        [StringLength(50)]
        public string SalaryVariableFor { get; set; } // Employee / Grade / Designation / All
        public long? EmployeeId { get; set; }
        public long? DesignationId { get; set; }
        public long? GradeId { get; set; }
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

        [ForeignKey("PeriodicallyVariableDeductionInfo")]
        public long PeriodicallyVariableDeductionInfoId { get; set; }
        public PeriodicallyVariableDeductionInfo PeriodicallyVariableDeductionInfo { get; set; }

    }
}
