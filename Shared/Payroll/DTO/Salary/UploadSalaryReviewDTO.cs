using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Shared.Payroll.DTO.Salary
{
    public class UploadSalaryReviewReadDTO
    {
        public long SalaryReviewInfoId { get; set; }
        public string EmployeeCode { get; set; }
        public long EmployeeId { get; set; }
        public long? DesignationId { get; set; }
        public long? InternalDesignationId { get; set; }
        public long? DepartmentId { get; set; }
        public long? SectionId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonthlyCTC { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonthlyPF { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonthlyFB { get; set; }
        public long? SalaryConfigCategoryInfoId { get; set; }
        [StringLength(20)]
        public string BaseType { get; set; } // GROSS / BASIC / FLAT / CTC
        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentSalaryAmount { get; set; } // CTC Amount
        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal SalaryBaseAmount { get; set; } // CTC Amount
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PreviousSalaryAmount { get; set; }
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
        public bool? IsAutoCalculate { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public Nullable<DateTime> EffectiveFrom { get; set; }
        public Nullable<DateTime> EffectiveTo { get; set; }
        public Nullable<DateTime> ActivationDate { get; set; }
        public Nullable<DateTime> DeactivationDate { get; set; }
        public bool IsArrearCalculated { get; set; }
        public Nullable<DateTime> ArrearCalculatedDate { get; set; }
        public long PreviousReviewId { get; set; }
        public List<UploadSalaryReviewDetailDTO> uploadSalaryReviewDetails { get; set; } = new();

    }

    public class UploadSalaryReviewDetailDTO : BaseModel1
    {
        public long SalaryReviewDetailId { get; set; }
        public long EmployeeId { get; set; }
        public long AllowanceNameId { get; set; }
        [StringLength(100)]
        public string AllowanceName { get; set; }
        public long? SalaryAllowanceConfigDetailId { get; set; }
        public long? SalaryConfigCategoryInfoId { get; set; }
        [StringLength(50)]
        public string SalaryConfigCategory { get; set; }
        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal SalaryBaseAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PreviousSalaryAmount { get; set; }
        [StringLength(20)]
        public string BaseType { get; set; }
        public long? PreviousReviewId { get; set; }

        [StringLength(50)]
        public string AllowanceBase { get; set; } // Gross / Basic / Flat / CTC

        [Column(TypeName = "decimal(18,6)")]
        public decimal? AllowancePercentage { get; set; } // Gross / Basic
        [Column(TypeName = "decimal(18,2)")]
        public decimal? AllowanceAmount { get; set; } // CTC / Flat
        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PreviousAmount { get; set; }
   
        [Column(TypeName = "decimal(18,2)")]
        public decimal? AdditionalAmount { get; set; }   
        public long SalaryReviewInfoId { get; set; }
        public UploadSalaryReviewInsertDTO uploadSalaryReviewInsertDTO { get; set; }
    }

    public class UploadSalaryReviewInsertDTO : BaseModel4
    {
        public long SalaryReviewInfoId { get; set; }
        public string EmployeeCode { get; set; }
        public long EmployeeId { get; set; }
        public long? DesignationId { get; set; }
        public long? InternalDesignationId { get; set; }
        public long? DepartmentId { get; set; }
        public long? SectionId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonthlyCTC { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonthlyPF { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonthlyFB { get; set; }
        public long? SalaryConfigCategoryInfoId { get; set; }
        [StringLength(20)]
        public string BaseType { get; set; } // GROSS / BASIC / FLAT / CTC
        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentSalaryAmount { get; set; } // CTC Amount
        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal? SalaryBaseAmount { get; set; } // CTC Amount
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PreviousSalaryAmount { get; set; }
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
        public bool? IsAutoCalculate { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public Nullable<DateTime> EffectiveFrom { get; set; }
        public Nullable<DateTime> EffectiveTo { get; set; }
        public Nullable<DateTime> ActivationDate { get; set; }
        public Nullable<DateTime> DeactivationDate { get; set; }
        public bool IsArrearCalculated { get; set; }
        public Nullable<DateTime> ArrearCalculatedDate { get; set; }
        public long? PreviousReviewId { get; set; }

        public List<UploadSalaryReviewDetailDTO> uploadSalaryReviewDetails { get; set; } = new();
    }
}
