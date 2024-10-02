using Shared.Models;
using Shared.Payroll.Domain.Allowance;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Salary
{
    [Table("Payroll_SalaryAllowanceArrearAdjustment")]
    public class SalaryAllowanceArrearAdjustment : BaseModel3
    {
        [Key]
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        [ForeignKey("AllowanceName")]
        public long AllowanceNameId { get; set; }
        public AllowanceName AllowanceName { get; set; }
        public short? SalaryMonth { get; set; }
        public short? SalaryYear { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> SalaryDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CalculationForDays { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [StringLength(50)]
        public string Flag { get; set; }
        public short? ArrearAdjustmentMonth { get; set; }
        public short? ArrearAdjustmentYear { get; set; }
        public bool IsActive { get; set; } = true;
        [StringLength(50)]
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; } = false;
        public long? SalaryProcessId { get; set; }
        public long? SalaryProcessDetailId { get; set; }
        [StringLength(100)]
        public string SalaryProcessUniqueId { get; set; }
    }
}
