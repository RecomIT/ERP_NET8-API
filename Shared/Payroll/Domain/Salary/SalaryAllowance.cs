using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Salary
{
    [Table("Payroll_SalaryAllowance"), Index("EmployeeId", "SalaryMonth", "SalaryYear", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_SalaryAllowance_NonClusteredIndex")]
    public class SalaryAllowance : BaseModel1
    {
        [Key]
        public long SalaryAllowanceId { get; set; }
        public long SalaryProcessId { get; set; }
        public long SalaryProcessDetailId { get; set; }
        public short SalaryMonth { get; set; }
        public short SalaryYear { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> SalaryDate { get; set; }
        [Column(TypeName = "decimal(4,2)")]
        public decimal CalculationForDays { get; set; }
        public long EmployeeId { get; set; }
        public long AllowanceNameId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ArrearAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal AdjustmentAmount { get; set; }
        public long? PeriodicallyAllowanceId { get; set; }
        public string PeriodicallyAllowanceIds { get; set; }
        public long? MonthlyAllowanceId { get; set; }
        public string MonthlyAllowanceIds { get; set; }
        public long? FiscalYearId { get; set; }
        public long? SalaryReviewInfoId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? BaseAmount { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        [StringLength(50)]
        public string SalaryProcessUniqId { get; set; }
    }
}
