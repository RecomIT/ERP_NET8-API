using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Salary
{
    [Table("Payroll_SalaryDeduction"), Index("EmployeeId", "SalaryMonth", "SalaryYear", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_SalaryDeduction_NonClusteredIndex")]
    public class SalaryDeduction : BaseModel1
    {
        [Key]
        public long SalaryDeductionId { get; set; }
        public long SalaryProcessId { get; set; }
        public long SalaryProcessDetailId { get; set; }
        public short SalaryMonth { get; set; }
        public short SalaryYear { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> SalaryDate { get; set; }
        [Column(TypeName = "decimal(4,2)")]
        public decimal CalculationForDays { get; set; }
        public long EmployeeId { get; set; }
        public long DeductionNameId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ArrearAmount { get; set; }
        public long? PeriodicallyDeductionId { get; set; }
        public string PeriodicallyDeductionIds { get; set; }
        public long? MonthlyDeductionId { get; set; }
        public string MonthlyDeductionIds { get; set; }
        public long? FiscalYearId { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        [StringLength(50)]
        public string SalaryProcessUniqId { get; set; }
    }
}
