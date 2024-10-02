using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Salary
{
    [Table("Payroll_SalaryReviewInfo"), Index("EmployeeId", "BaseType", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_SalaryReviewInfo_NonClusteredIndex")]
    public class SalaryReviewInfo : BaseModel4
    {
        [Key]
        public long SalaryReviewInfoId { get; set; }
        [Required]
        public long EmployeeId { get; set; }
        public long? DesignationId { get; set; }
        public long? InternalDesignationId { get; set; }
        public long? DepartmentId { get; set; }
        public long? SectionId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? YearlyCTC { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonthlyCTC { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonthlyPF { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonthlyFB { get; set; }
        [StringLength(20)]
        public string BaseType { get; set; } // GROSS / BASIC / FLAT / CTC
        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentSalaryAmount { get; set; } // CTC Amount
        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal SalaryBaseAmount { get; set; } // CTC Amount
        [Column(TypeName = "decimal(18,2)")]
        public decimal PreviousSalaryAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CTCAmountWithoutFestivalBonus { get; set; }
        public long? SalaryAllowanceConfigId { get; set; }
        [StringLength(50)]
        public string SalaryConfigCategory { get; set; }
        [StringLength(100)]
        public string IncrementReason { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        public bool IsActive { get; set; }
        public bool? IsApproved { get; set; }
        public bool IsAutoCalculate { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> EffectiveFrom { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> EffectiveTo { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> ActivationDate { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> DeactivationDate { get; set; }
        public bool IsArrearCalculated { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> ArrearCalculatedDate { get; set; }
        public long? PreviousReviewId { get; set; }
        public ICollection<SalaryReviewDetail> SalaryReviewDetails { get; set; }
    }
}
