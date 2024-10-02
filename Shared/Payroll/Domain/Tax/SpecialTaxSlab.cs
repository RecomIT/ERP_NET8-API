using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Tax
{
    [Table("Payroll_SpecialTaxSlab"), Index("EmployeeId", "FiscalYearId", "ImpliedCondition", "StateStatus", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_SpecialTaxSlab_NonClusteredIndex")]
    public class SpecialTaxSlab : BaseModel
    {
        [Key]
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public long FiscalYearId { get; set; }
        [StringLength(20)]
        public string AssessmentYear { get; set; }
        [StringLength(20)]
        public string ImpliedCondition { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SlabMininumAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SlabMaximumAmount { get; set; }
        [Column(TypeName = "decimal(4,2)")]
        public decimal SlabPercentage { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
    }
}
