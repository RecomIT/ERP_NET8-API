using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Salary
{
    [Table("Payroll_SalaryDeductionAdjustment"), Index("DeductionNameId", "EmployeeId", "SalaryMonth", "SalaryYear", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_SalaryDeductionAdjustment_NonClusteredIndex")]
    public class SalaryDeductionAdjustment : BaseModel3
    {
        [Key]
        public long DeductionAdjustmentId { get; set; }
        public long SalaryProcessId { get; set; }
        public long SalaryProcessDetailId { get; set; }
        public long EmployeeId { get; set; }
        public long DeductionNameId { get; set; }
        public short SalaryMonth { get; set; }
        public short SalaryYear { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> SalaryDate { get; set; }
        [Column(TypeName = "decimal(4,2)")]
        public decimal? CalculationForDays { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public short AdjustmentMonth { get; set; }
        public short AdjustmentYear { get; set; }
        [StringLength(50)]
        public string Origin { get; set; } // PROCESS / INPUT
        [StringLength(200)]
        public string Remarks { get; set; }
        public long? FiscalYearId { get; set; }
        [StringLength(50)]
        public string SalaryProcessUniqId { get; set; }
        public bool? IsApproved { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
    }
}
