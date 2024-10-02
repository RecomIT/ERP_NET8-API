using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Setup
{
    [Table("Payroll_FiscalYear"), Index("FiscalYearRange", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_FiscalYear_NonClusteredIndex")]
    public class FiscalYear : BaseModel2
    {
        [Key]
        public long FiscalYearId { get; set; }
        [StringLength(100)]
        public string FiscalYearRange { get; set; }
        [StringLength(100)]
        public string AssesmentYear { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> FiscalYearFrom { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> FiscalYearTo { get; set; }
        public long? AuthorizedBy { get; set; }
        public long? AuthorizerDesignationId { get; set; }
        [MaxLength]
        public byte[] AuthorizedSignForTax { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }
    }
}
