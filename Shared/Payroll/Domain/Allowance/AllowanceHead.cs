using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Allowance
{
    [Table("Payroll_AllowanceHead"), Index("AllowanceHeadName", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_AllowanceHead_NonClusteredIndex")]
    public class AllowanceHead : BaseModel1
    {
        [Key]
        public long AllowanceHeadId { get; set; }
        [Required, StringLength(200)]
        public string AllowanceHeadName { get; set; }
        [StringLength(20)]
        public string AllowanceHeadCode { get; set; }
        [StringLength(200)]
        public string AllowanceHeadNameInBengali { get; set; }
        public bool IsActive { get; set; }
        public ICollection<AllowanceName> AllowanceNames { get; set; }
    }
}
