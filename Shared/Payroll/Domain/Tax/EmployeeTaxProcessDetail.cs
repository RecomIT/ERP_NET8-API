using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Tax
{
    [Table("Payroll_EmployeeTaxProcessDetail"), Index("EmployeeId", "AllowanceNameId", "FiscalYearId", "SalaryMonth", "SalaryYear", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_EmployeeTaxProcessDetail_NonClusteredIndex")]
    public class EmployeeTaxProcessDetail : BaseModel1
    {
        [Key]
        public long TaxProcessDetailId { get; set; }
        public long TaxProcessId { get; set; }
        public long EmployeeId { get; set; }
        public long AllowanceHeadId { get; set; }
        [StringLength(200)]
        public string AllowanceHeadName { get; set; }
        public long AllowanceNameId { get; set; }
        [StringLength(200)]
        public string AllowanceName { get; set; }
        public long BonusId { get; set; }
        [StringLength(100)]
        public string BonusName { get; set; }
        [StringLength(100)]
        public string TaxItem { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TillDateIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentMonthIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ProjectedIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TillAdjusmentAmount { get; set; } // Adjustment
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CurrentAdjusmentAmount { get; set; } // Adjustment
        [Column(TypeName = "decimal(18,2)")]
        public decimal GrossAnnualIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal LessExempted { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalTaxableIncome { get; set; }
        public bool? IsPerquisite { get; set; }
        public long SalaryProcessId { get; set; }
        public long? SalaryProcessDetailId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ArrearAmount { get; set; }
        public bool? IsProjected { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }
        public long? FiscalYearId { get; set; }
        public short SalaryMonth { get; set; }
        public short SalaryYear { get; set; }
        public long? AllowanceConfigId { get; set; }
        // Deduction
        public long? DeductionHeadId { get; set; }
        [StringLength(200)]
        public string DeductionHeadName { get; set; }
        public long? DeductionNameId { get; set; }
        [StringLength(200)]
        public string DeductionNameName { get; set; }
        public long? DeductionConfigId { get; set; }
    }
}
