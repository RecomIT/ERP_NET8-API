using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Tax
{
    [Table("Payroll_IncomeTaxSlab"), Index("FiscalYearId", "ImpliedCondition", "StateStatus", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_IncomeTaxSlab_NonClusteredIndex")]
    public class IncomeTaxSlab : BaseModel2
    {
        [Key]
        public long IncomeTaxSlabId { get; set; }
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
