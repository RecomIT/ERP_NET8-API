using System;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Shared.Payroll.Domain.Deduction;

namespace Shared.Payroll.Domain.Variable
{
    [Table("Payroll_MonthlyVariableDeduction"), Index("EmployeeId", "SalaryMonth", "SalaryYear", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_MonthlyVariableDeduction_NonClusteredIndex")]
    public class MonthlyVariableDeduction : BaseModel3
    {
        [Key]
        public long MonthlyVariableDeductionId { get; set; }
        [Required]
        public long EmployeeId { get; set; }
        public long? DesignationId { get; set; }
        [Column(TypeName = "date")]
        public DateTime? SalaryMonthYear { get; set; }
        public short? SalaryMonth { get; set; }
        public short? SalaryYear { get; set; }
        [StringLength(50)]
        public string AmountBaseOn { get; set; } // Gross / Basic / Flat / Principal
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Percentage { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EffectiveFrom { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EffectiveTo { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }
        [ForeignKey("DeductionName")]
        public long DeductionNameId { get; set; }
        public DeductionName DeductionName { get; set; }
    }
}
