using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Bonus
{
    [Table("Payroll_BonusProcessDetail"), Index("EmployeeId", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_BonusProcessDetail_NonClusteredIndex")]
    public class BonusProcessDetail : BaseModel1
    {
        [Key]
        public long BonusProcessDetailId { get; set; }
        [StringLength(50)]
        public string BatchNo { get; set; }
        public long EmployeeId { get; set; }
        public long? DepartmentId { get; set; }
        public long? DesignationId { get; set; }
        public long? SectionId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? OnceOffTax { get; set; }
        public short BonusMonth { get; set; }
        public short BonusYear { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> ProcessDate { get; set; }
        [ForeignKey("BonusProcess")]
        public long BonusProcessId { get; set; }
        public BonusProcess BonusProcess { get; set; }
    }
}
