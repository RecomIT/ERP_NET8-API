using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Tax
{
    [Table("Payroll_TaxChallan"), Index("EmployeeId", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_TaxChallan_NonClusteredIndex")]
    public class TaxChallan : BaseModel1
    {
        [Key]
        public long TaxChallanId { get; set; }
        public short Month { get; set; } = 0;
        public Nullable<DateTime> TaxDate { get; set; }
        public string TaxMonth { get; set; }
        public short TaxYear { get; set; }
        public long? EmployeeId { get; set; }
        public long? FiscalYearId { get; set; }
        [StringLength(500)]
        public string FilePath { get; set; }
        [StringLength(50)]
        public string FileFormat { get; set; }
        [StringLength(150)]
        public string ChallanNumber { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> ChallanDate { get; set; }
        [StringLength(200)]
        public string DepositeBank { get; set; }
        [StringLength(200)]
        public string DepositeBranch { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
    }
}
