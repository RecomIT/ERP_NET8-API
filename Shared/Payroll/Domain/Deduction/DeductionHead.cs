using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Deduction
{
    [Table("Payroll_DeductionHead"), Index("DeductionHeadName", "IsActive", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_DeductionHead_NonClusteredIndex")]
    public class DeductionHead : BaseModel1
    {
        [Key]
        public long DeductionHeadId { get; set; }
        [Required, StringLength(200)]
        public string DeductionHeadName { get; set; }
        [StringLength(20)]
        public string DeductionHeadCode { get; set; }
        [StringLength(200)]
        public string DeductionHeadNameInBengali { get; set; }
        public bool IsActive { get; set; }
    }
}
