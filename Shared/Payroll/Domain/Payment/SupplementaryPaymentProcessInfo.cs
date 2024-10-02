using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Payment
{
    [Table("Payroll_SupplementaryPaymentProcessInfo"), Index("FiscalYearId", "PaymentMonth", "PaymentYear", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_SupplementaryPaymentProcessInfo_NonClusteredIndex")]
    public class SupplementaryPaymentProcessInfo : BaseModel2
    {
        [Key]
        public long PaymentProcessInfoId { get; set; }
        [StringLength(50)]
        public string BatchNo { get; set; }
        public long? AllowanceNameId { get; set; }
        public long? FiscalYearId { get; set; }
        public int TotalEmployees { get; set; }
        public short PaymentMonth { get; set; }
        public short PaymentYear { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> CutOffDate { get; set; }
        public bool EffectOnPayslip { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalOnceOffAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalTax { get; set; }
        public bool? IsDisbursed { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> PaymentDate { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }
        [StringLength(100)]
        public string ProcessType { get; set; }
        public bool? ShowInPayslip { get; set; }
        public bool? ShowInSalarySheet { get; set; }
        [StringLength(50)]
        public string PaymentMode { get; set; }
        [StringLength(100)]
        public string PaymentHeadName { get; set; }
        [StringLength(100)]
        public string PayableHeadName { get; set; }
        public short? PayableMonth { get; set; }
        public short? PayableYear { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> PayableDate { get; set; }
    }
}
