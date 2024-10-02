using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Salary
{
    [Table("Payroll_SalaryAllowanceArrear"), Index("EmployeeId", "SalaryMonth", "SalaryYear", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_SalaryAllowanceArrear_NonClusteredIndex")]
    public class SalaryAllowanceArrear : BaseModel3
    {
        [Key]
        public long SalaryAllowanceArrearId { get; set; }
        public long SalaryProcessId { get; set; }
        public long SalaryProcessDetailId { get; set; }
        public long EmployeeId { get; set; }
        public long AllowanceNameId { get; set; }
        public short SalaryMonth { get; set; }
        public short SalaryYear { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> SalaryDate { get; set; }
        [Column(TypeName = "decimal(4,2)")]
        public decimal? CalculationForDays { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public short ArrearMonth { get; set; }
        public short ArrearYear { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> ArrearFrom { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> ArrearTo { get; set; }
        [StringLength(50)]
        public string Origin { get; set; } // PROCESS / INPUT
        [StringLength(200)]
        public string Remarks { get; set; }
        [StringLength(50)]
        public string SalaryProcessUniqId { get; set; }
        public long? FiscalYearId { get; set; }
        public bool? IsApproved { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        public long? SalaryReviewInfoId { get; set; }
    }
}
