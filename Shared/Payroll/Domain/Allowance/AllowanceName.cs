using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Allowance
{
    [Table("Payroll_AllowanceName"), Index("Name", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_AllowanceName_NonClusteredIndex")]
    public class AllowanceName : BaseModel1
    {
        [Key]
        public long AllowanceNameId { get; set; }
        [Required, StringLength(200)]
        public string Name { get; set; }
        [StringLength(20)]
        public string GLCode { get; set; }
        [StringLength(200)]
        public string AllowanceNameInBengali { get; set; }
        [StringLength(200)]
        public string AllowanceClientName { get; set; }
        [StringLength(200)]
        public string AllowanceClientNameInBengali { get; set; }
        [StringLength(300)]
        public string AllowanceDescription { get; set; }
        [StringLength(300)]
        public string AllowanceDescriptionInBengali { get; set; }
        [StringLength(50)]
        public string AllowanceType { get; set; } // General / Salary
        public long? SerialNo { get; set; }
        public bool? IsFixed { get; set; }
        [StringLength(100)]
        public string Flag { get; set; } // BASIC / HR / CONVEYANCE / MEDICAL / CHILD / LFA / LWP / GF
        public bool IsActive { get; set; }
        [ForeignKey("AllowanceHead")]
        public long AllowanceHeadId { get; set; }
        public AllowanceHead AllowanceHead { get; set; }
    }
}
