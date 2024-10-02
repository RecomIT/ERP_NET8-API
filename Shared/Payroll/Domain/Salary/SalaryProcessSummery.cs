using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Salary
{
    [Table("Payroll_SalaryProcessSummery"), Index("SalaryMonth", "SalaryYear", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_SalaryProcessSummery_NonClusteredIndex")]
    public class SalaryProcessSummery : BaseModel
    {
        [Key]
        public long SalaryProcessSummeryId { get; set; }
        [StringLength(50)]
        public string HeadType { get; set; } // BRANCH/DEPARTMENT/SECTION/GRADE
        public long HeadId { get; set; } // BRANCH-ID/DEPARTMENT-ID/SECTION-ID/GRADE-ID
        public short SalaryMonth { get; set; }
        public short SalaryYear { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAllowance { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalArrearAllowance { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAllowanceAdjustment { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PFAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PFArrear { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalDeduction { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalArrearDeduction { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ProjectionTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal OnceOffTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalMonthlyTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalTaxDeductedAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalBonus { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal GrossPay { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal NetPay { get; set; }
    }
}
