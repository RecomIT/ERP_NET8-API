using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Bonus
{
    [Table("Payroll_EmployeeBonusTaxProcessSlab"), Index("EmployeeId", "FiscalYearId", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_EmployeeBonusTaxProcessSlab_NonClusteredIndex")]
    public class EmployeeBonusTaxProcessSlab : BaseModel1
    {
        public long EmployeeBonusTaxProcessSlabId { get; set; }
        public long EmployeeId { get; set; }
        public long BonusId { get; set; }
        public long BonusConfigId { get; set; }
        public long BonusProcessId { get; set; }
        public long? FiscalYearId { get; set; }
        public short BonusMonth { get; set; }
        public short BonusYear { get; set; }
        public long? IncomeTaxSlabId { get; set; }
        [StringLength(20)]
        public string ImpliedCondition { get; set; }
        [Column(TypeName = "decimal(4,2)")]
        public decimal SlabPercentage { get; set; }
        [StringLength(100)]
        public string ParameterName { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TaxableIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TaxLiability { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> BonusDate { get; set; }
    }
}
