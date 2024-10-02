using System;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Shared.Payroll.Domain.Allowance;

namespace Shared.Payroll.Domain.Variable
{
    [Table("Payroll_MonthlyVariableAllowance"), Index("EmployeeId", "SalaryMonth", "SalaryYear", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_MonthlyVariableAllowance_NonClusteredIndex")]
    public class MonthlyVariableAllowance : BaseModel2
    {
        [Key]
        public long MonthlyVariableAllowanceId { get; set; }
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
        [ForeignKey("AllowanceName")]
        public long AllowanceNameId { get; set; }
        public AllowanceName AllowanceName { get; set; }
    }
}
