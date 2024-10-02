using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Allowance
{
    [Table("Payroll_SalaryAllowanceConfigurationInfo"), Index("ConfigCategory", "BaseType", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_SalaryAllowanceConfigurationInfo_NonClusteredIndex")]
    public class SalaryAllowanceConfigurationInfo : BaseModel4
    {
        [Key]
        public long SalaryAllowanceConfigId { get; set; }
        [StringLength(100)]
        public string ConfigCategory { get; set; }
        public string BreakdownName { get; set; }
        public int? HeadCount { get; set; }
        [StringLength(100)]
        public string BaseType { get; set; }
        public bool IsActive { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        [StringLength(50)]
        public string JobType { get; set; }
        public ICollection<SalaryAllowanceConfigurationDetail> SalaryAllowanceConfigurationDetails { get; set; }
    }
}
