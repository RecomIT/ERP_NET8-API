using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Salary
{
    [Table("Payroll_SalaryReviewDetail"), Index("EmployeeId", "AllowanceNameId", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_SalaryReviewDetail_NonClusteredIndex")]
    public class SalaryReviewDetail : BaseModel1
    {
        [Key]
        public long SalaryReviewDetailId { get; set; }
        public long? EmployeeId { get; set; }
        public long AllowanceNameId { get; set; }
        [StringLength(100)]
        public string AllowanceName { get; set; }
        public long? SalaryAllowanceConfigDetailId { get; set; }
        [StringLength(50)]
        public string AllowanceBase { get; set; } // Gross / Basic / Flat / CTC
        [Column(TypeName = "decimal(18,2)")]
        public decimal? AllowancePercentage { get; set; } // Gross / Basic
        [Column(TypeName = "decimal(18,2)")]
        public decimal? AllowanceAmount { get; set; } // CTC / Flat
        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentAmount { get; set; } //
        [Column(TypeName = "decimal(18,2)")]
        public decimal PreviousAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? AdditionalAmount { get; set; }
        [ForeignKey("SalaryReviewInfo")]
        public long SalaryReviewInfoId { get; set; }
        public SalaryReviewInfo SalaryReviewInfo { get; set; }
    }
}
