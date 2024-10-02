using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Bonus
{
    [Table("Payroll_Bonus"), Index("BonusName", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_Bonus_NonClusteredIndex")]
    public class Bonus : BaseModel1
    {
        [Key]
        public long BonusId { get; set; }
        [StringLength(150)]
        public string BonusName { get; set; }
        [StringLength(50)]
        public string BonusState { get; set; }
        public bool IsActive { get; set; }
        [StringLength(50)]
        public string ActivatedBy { get; set; }
        public Nullable<DateTime> ActivationDate { get; set; }
        [StringLength(50)]
        public string DeactivatedBy { get; set; }
        public Nullable<DateTime> DeactivationDate { get; set; }
        [StringLength(100)]
        public string Reason { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public ICollection<BonusConfig> BonusConfigs { get; set; }
        public ICollection<BonusProcess> BonusProcesses { get; set; }
    }
}
