using System;
using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Tax
{
    [Table("Payroll_EmployeeTaxZone"), Index("EmployeeId", "FiscalYearId", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_EmployeeTaxZone_NonClusteredIndex")]
    public class EmployeeTaxZone : BaseModel3
    {
        [Key]
        public long EmployeeTaxZoneId { get; set; }
        [Required]
        public long EmployeeId { get; set; }
        public long? FiscalYearId { get; set; }
        [Required, StringLength(100)]
        public string Taxzone { get; set; }
        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal? MinimumTaxAmount { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> EffectiveDate { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> ActiveDate { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> InActiveDate { get; set; }
        [StringLength(50)]
        public bool IsApproved { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
    }
}
