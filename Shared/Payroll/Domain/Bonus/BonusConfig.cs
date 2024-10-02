using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Bonus
{
    [Table("Payroll_BonusConfig"), Index("BonusConfigCode", "FiscalYearId", "StateStatus", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_BonusConfig_NonClusteredIndex")]
    public class BonusConfig : BaseModel3
    {
        [Key]
        public long BonusConfigId { get; set; }
        [StringLength(50)]
        public string BonusConfigCode { get; set; }
        public long FiscalYearId { get; set; }
        public bool IsReligious { get; set; }
        [StringLength(100)]
        public string ReligionName { get; set; }
        public bool? IsConfirmedEmployee { get; set; }
        public bool? IsFestival { get; set; }
        public bool? IsTaxable { get; set; }
        [StringLength(100)]
        public string TaxConditionType { get; set; }
        public bool? IsPaymentProjected { get; set; }
        public bool? IsTaxDistributed { get; set; }
        public bool? IsOnceOff { get; set; }
        public bool? IsExcludeFromSalary { get; set; } // If IsOnceOff true then by IsExcludeSalary is false
        public bool? IsTaxAdjustedWithSalary { get; set; } //  If IsExcludeSalary is true then IsTaxAdjustedWithSalary is false by default
        public bool? IsPerquisite { get; set; }
        public short BonusCount { get; set; }
        [StringLength(50)]
        public string BasedOn { get; set; } // GROSS // BASIC // FLAT
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Percentage { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaximumAmount { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        [ForeignKey("BonusConfig")]
        public long BonusId { get; set; }
        public Bonus Bonus { get; set; }
    }
}
