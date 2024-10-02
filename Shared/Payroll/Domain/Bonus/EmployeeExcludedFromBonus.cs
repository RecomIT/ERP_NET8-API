using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Bonus
{
    [Table("Payroll_EmployeeExcludedFromBonus"), Index("EmployeeId", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_EmployeeExcludedFromBonus_NonClusteredIndex")]
    public class EmployeeExcludedFromBonus : BaseModel2
    {
        [Key]
        public long ExcludeId { get; set; }
        [Required]
        public long EmployeeId { get; set; }
        [Required]
        public long BonusId { get; set; }
        [Required]
        public long BonusConfigId { get; set; }
    }
}
