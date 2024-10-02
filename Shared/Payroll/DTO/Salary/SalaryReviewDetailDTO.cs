using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.DTO.Salary
{
    public class SalaryReviewDetailDTO
    {
        public long SalaryReviewDetailId { get; set; }
        [Range(1, long.MaxValue)]
        public long AllowanceNameId { get; set; }
        [StringLength(100)]
        public string AllowanceName { get; set; }
        public long? SalaryAllowanceConfigDetailId { get; set; }
        [Required, StringLength(50)]
        public string AllowanceBase { get; set; } // Gross / Basic / Flat
        [Column(TypeName = "decimal(18,2)")]
        public decimal? AllowancePercentage { get; set; } // Gross / Basic
        [Column(TypeName = "decimal(18,2)")]
        public decimal? AllowanceAmount { get; set; } // Flat
        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal CurrentAmount { get; set; } //
        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal PreviousAmount { get; set; }
        public long SalaryReviewInfoId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaxAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MinAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? AdditionalAmount { get; set; }
        public long AllowanceHeadId { get; set; }
        public string AllowanceHeadName { get; set; }
        public long? SalaryConfigCategoryInfoId { get; set; }
        [StringLength(50)]
        public string SalaryConfigCategory { get; set; }
        [StringLength(20)]
        public string AllowanceFlag { get; set; }
        public long? EmployeeId { get; set; }
    }
}
