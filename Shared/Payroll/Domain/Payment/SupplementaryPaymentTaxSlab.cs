using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Payment
{
    [Table("Payroll_SupplementaryPaymentTaxSlab"), Index("EmployeeId", "FiscalYearId", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_SupplementaryPaymentTaxSlab_NonClusteredIndex")]
    public class SupplementaryPaymentTaxSlab : BaseModel1
    {
        public long SupplementaryPaymentTaxSlabId { get; set; }
        public long EmployeeId { get; set; }
        public long? FiscalYearId { get; set; }
        public short PaymentMonth { get; set; }
        public short PaymentYear { get; set; }
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
        public Nullable<DateTime> PaymentDate { get; set; }
        public long PaymentTaxInfoId { get; set; }
        public long PaymentAmountId { get; set; } = 0;
        public long PaymentProcessInfoId { get; set; } = 0;
        public short? PayableMonth { get; set; }
        public short? PayableYear { get; set; }
        public Nullable<DateTime> PayableDate { get; set; }
    }
}
