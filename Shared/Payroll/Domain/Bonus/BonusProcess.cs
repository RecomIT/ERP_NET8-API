using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Bonus
{
    [Table("Payroll_BonusProcess"), Index("FiscalYearId", "BonusMonth", "BonusYear", "BatchNo", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_BonusProcess_NonClusteredIndex")]
    public class BonusProcess : BaseModel4
    {
        [Key]
        public long BonusProcessId { get; set; }
        [ForeignKey("BonusConfig")]
        public long BonusConfigId { get; set; }
        public BonusConfig BonusConfig { get; set; }
        public long FiscalYearId { get; set; }
        public short BonusMonth { get; set; }
        public short BonusYear { get; set; }
        public bool IsDisbursed { get; set; }
        public int TotalEmployees { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalTax { get; set; }
        [StringLength(50)]
        public string BatchNo { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> ProcessDate { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> PaymentDate { get; set; }
        public long? DepartmentId { get; set; }
        public ICollection<BonusProcessDetail> BonusProcessDetails { get; set; }
    }

}
