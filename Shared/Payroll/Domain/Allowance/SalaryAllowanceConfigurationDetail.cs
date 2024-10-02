using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Allowance
{
    [Table("Payroll_SalaryAllowanceConfigurationDetails")]
    public class SalaryAllowanceConfigurationDetail : BaseModel2
    {
        [Key]
        public long SalaryAllowanceConfigDetailId { get; set; }
        [StringLength(100)]
        public string AllowanceBase { get; set; }
        public long AllowanceNameId { get; set; }
        [StringLength(100)]
        public string AllowanceName { get; set; }
        [StringLength(100)]
        public string DependentAllowance { get; set; }
        public long? EmployeeId { get; set; }
        [StringLength(100)]
        public string JobType { get; set; }
        public long? GradeId { get; set; }
        public long? DesignationId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Percentage { get; set; }
        public bool IsActive { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaxAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MinAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? AdditionalAmount { get; set; }
        public bool IsPeriodically { get; set; }
        public Nullable<DateTime> EffectiveFrom { get; set; }
        public Nullable<DateTime> EffectiveTo { get; set; }
        [ForeignKey("SalaryAllowanceConfigurationInfo")]
        public long SalaryAllowanceConfigId { get; set; }
        public SalaryAllowanceConfigurationInfo SalaryAllowanceConfigurationInfo { get; set; }
    }
}
