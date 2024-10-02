using System;
using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Tax
{
    [Table("Payroll_EmployeeTaxReturnSubmission"), Index("EmployeeId", "FiscalYearId", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_EmployeeTaxReturnSubmission_NonClusteredIndex")]
    public class EmployeeTaxReturnSubmission : BaseModel2
    {
        [Key]
        public long TaxSubmissionId { get; set; }
        [Required]
        public long EmployeeId { get; set; }
        [Required]
        public long FiscalYearId { get; set; }
        [Required, StringLength(100)]
        public string RegistrationNo { get; set; }
        [Required, StringLength(100)]
        public string TaxZone { get; set; }
        [Required, StringLength(100)]
        public string TaxCircle { get; set; }
        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal PaidAmount { get; set; }
        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal TaxPayable { get; set; }
        [Required, Column(TypeName = "date")]
        public Nullable<DateTime> SubmissionDate { get; set; }
        [StringLength(200)]
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}
