using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Bonus
{
    [Table("Payroll_EmployeeBonusTaxProcessDetail"), Index("EmployeeId", "FiscalYearId", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_EmployeeBonusTaxProcessDetail_NonClusteredIndex")]
    public class EmployeeBonusTaxProcessDetail : BaseModel1
    {
        [Key]
        public long EmployeeBonusTaxProcessDetailId { get; set; }
        public long EmployeeId { get; set; }
        public long AllowanceHeadId { get; set; }
        [StringLength(200)]
        public string AllowanceHeadName { get; set; }
        public long AllowanceNameId { get; set; }
        [StringLength(200)]
        public string AllowanceName { get; set; }
        public long BonusId { get; set; }
        public long BonusConfigId { get; set; }
        public long BonusProcessId { get; set; }
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
        public decimal GrossAnnualIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal LessExempted { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalTaxableIncome { get; set; }
        public bool? IsPerquisite { get; set; }
        public long SalaryProcessId { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }
        public long? FiscalYearId { get; set; }
        public short BonusMonth { get; set; }
        public short BonusYear { get; set; }
        public long? AllowanceConfigId { get; set; }
    }
}
