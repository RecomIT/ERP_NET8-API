using Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.ViewModel.Salary
{
    public class SalaryReviewInfoViewModel : BaseViewModel5
    {
        public long SalaryReviewInfoId { get; set; }
        [Required]
        public long EmployeeId { get; set; }
        public long? FiscalYearId { get; set; }
        public long? DesignationId { get; set; }
        public long? InternalDesignationId { get; set; }
        public long? DepartmentId { get; set; }
        public long? SectionId { get; set; }
        [StringLength(20)]
        public string BaseType { get; set; }
        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal CurrentSalaryAmount { get; set; }
        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal SalaryBaseAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PreviousSalaryAmount { get; set; }
        public long? SalaryAllowanceConfigId { get; set; }
        [Required, StringLength(50)]
        public string SalaryConfigCategory { get; set; }
        [Required, StringLength(100)]
        public string IncrementReason { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        public bool? IsApproved { get; set; }
        public bool IsActive { get; set; }
        public bool IsAutoCalculate { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EffectiveFrom { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EffectiveTo { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ActivationDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DeactivationDate { get; set; }
        public bool IsArrearCalculated { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ArrearCalculatedDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? YearlyCTC { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonthlyCTC { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonthlyPF { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonthlyFB { get; set; }
        public List<SalaryReviewDetailViewModel> SalaryReviewDetails { get; set; }
        // 
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public string DesignationName { get; set; }
        public string InternalDesignationName { get; set; }
        public string DepartmentName { get; set; }
        public string SectionName { get; set; }
        public short ActivationMonth { get; set; }
        public short ActivationYear { get; set; }
        public short ArrearMonth { get; set; }
        public short ArrearYear { get; set; }
        public long? PreviousReviewId { get; set; }
        ////public long? SalaryAllowanceConfigDetailId { get; set; }
        ////public long? SalaryConfigCategoryInfoId { get; set; }
    }
}
