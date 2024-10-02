using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Deduction
{
    [Table("Payroll_DeductionName"), Index("Name", "GLCode", "IsActive", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_DeductionName_NonClusteredIndex")]
    public class DeductionName : BaseModel1
    {
        [Key]
        public long DeductionNameId { get; set; }
        [Required, StringLength(200)]
        public string Name { get; set; }
        [StringLength(20)]
        public string GLCode { get; set; }
        [StringLength(200)]
        public string DeductionNameInBengali { get; set; }
        [StringLength(200)]
        public string DeductionClientName { get; set; }
        [StringLength(200)]
        public string DeductionClientNameInBengali { get; set; }
        [StringLength(300)]
        public string DeductionDescription { get; set; }
        [StringLength(300)]
        public string DeductionDescriptionInBengali { get; set; }
        [StringLength(50)]
        public string DeductionType { get; set; } // General / Salary
        public bool? IsFixed { get; set; }
        [StringLength(100)]
        public string Flag { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey("DeductionHead")]
        public long DeductionHeadId { get; set; }
        public DeductionHead DeductionHead { get; set; }
    }
}
