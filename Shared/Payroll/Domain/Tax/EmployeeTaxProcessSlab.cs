using System;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Shared.Payroll.Domain.Tax
{
    [Table("Payroll_EmployeeTaxProcessSlab"),
        Index("EmployeeId", "FiscalYearId", "SalaryMonth", "SalaryYear", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_EmployeeTaxProcessSlab_NonClusteredIndex")]
    public class EmployeeTaxProcessSlab : BaseModel1
    {
        public long EmployeeTaxProcessSlabId { get; set; }
        public long EmployeeId { get; set; }
        public long? FiscalYearId { get; set; }
        public short SalaryMonth { get; set; }
        public short SalaryYear { get; set; }
        public long? IncomeTaxSlabId { get; set; }
        [StringLength(20)]
        public string ImpliedCondition { get; set; }
        [Column(TypeName = "decimal(4,2)")]
        public decimal SlabPercentage { get; set; }
        [StringLength(100)]
        public string ParameterName { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TaxableIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TaxLiability { get; set; }
        public Nullable<DateTime> SalaryDate { get; set; }
        public long TaxProcessId { get; set; }
    }
}
