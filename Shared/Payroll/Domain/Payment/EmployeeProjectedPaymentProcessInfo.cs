using Microsoft.EntityFrameworkCore;
using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Payment
{
    [Table("Payroll_EmployeeProjectedAllowanceProcessInfo"), Index("FiscalYearId", "PaymentMonth", "PaymentYear", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_EmployeeProjectedAllowanceProcessInfo_NonClusteredIndex")]
    public class EmployeeProjectedAllowanceProcessInfo : BaseModel2
    {
        [Key]
        public long ProjectedAllowanceProcessInfoId { get; set; }
        [StringLength(100)]
        public string ProcessCode { get; set; }
        public int HeadCount { get; set; }
        public long? FiscalYearId { get; set; }
        public short PaymentMonth { get; set; }
        public short PaymentYear { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalPayableAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalDisbursedAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalTaxAmount { get; set; }
        [StringLength(100)]
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }
        public bool? IsDisbursed { get; set; }
        public bool? ShowInPayslip { get; set; }
        public bool? ShowInSalarySheet { get; set; }
        public bool? WithCOC { get; set; }
        [StringLength(200)]
        public string Reason { get; set; }
        [StringLength(50)]
        public string PaymentMode { get; set; }
        public short? PayableMonth { get; set; }
        public short? PayableYear { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> PaymentDate { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> PayableDate { get; set; }
        public ICollection<EmployeeProjectedPayment> EmployeeProjectedPayments { get; set; }
    }
}
