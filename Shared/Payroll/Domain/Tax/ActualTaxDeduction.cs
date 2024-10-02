using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Tax
{
    [Table("Payroll_ActualTaxDeduction"), Index("EmployeeId", "SalaryMonth", "SalaryYear", "FiscalYearId", "StateStatus", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_ActualTaxDeduction_NonClusteredIndex")]
    public class ActualTaxDeduction : BaseModel1
    {
        [Key]
        public long ActualTaxDeductionId { get; set; }
        public long FiscalYearId { get; set; }
        public long EmployeeId { get; set; }
        public short SalaryMonth { get; set; }
        public short SalaryYear { get; set; }
        [StringLength(1000)]
        public string FilePath { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ActualTaxAmount { get; set; }
        [StringLength(500)]
        public string ActualFileName { get; set; }
        [StringLength(500)]
        public string SystemFileName { get; set; }
        [StringLength(50)]
        public string FileFormat { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        public bool? IsApproved { get; set; }
        /// <summary>
        /// Added by Monzur 24-Jan-24
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ProjectionTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? OnceOffTax { get; set; }

    }
}
