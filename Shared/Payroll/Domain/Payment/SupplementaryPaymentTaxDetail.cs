using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Payroll.Domain.Payment
{
    [Table("Payroll_SupplementaryPaymentTaxDetail"), Index("EmployeeId", "AllowanceNameId", "FiscalYearId", "PaymentMonth", "PaymentYear", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_SupplementaryPaymentTaxDetail_NonClusteredIndex")]
    public class SupplementaryPaymentTaxDetail : BaseModel1
    {
        [Key]
        public long SupplementaryPaymentTaxDetailId { get; set; }
        public long PaymentAmountId { get; set; } = 0;
        public long PaymentProcessInfoId { get; set; } = 0;
        public long PaymentTaxInfoId { get; set; }
        public long EmployeeId { get; set; }
        public long AllowanceHeadId { get; set; }
        [StringLength(200)]
        public string AllowanceHeadName { get; set; }
        public long AllowanceNameId { get; set; }
        [StringLength(200)]
        public string AllowanceName { get; set; }
        [StringLength(100)]
        public string TaxItem { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TillDateIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentMonthIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ProjectedIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TillAdjusmentAmount { get; set; } // Adjustment
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CurrentAdjusmentAmount { get; set; } // Adjustment
        [Column(TypeName = "decimal(18,2)")]
        public decimal GrossAnnualIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal LessExempted { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalTaxableIncome { get; set; }
        public bool? IsPerquisite { get; set; }
        public long SalaryProcessId { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }
        public long? FiscalYearId { get; set; }
        public short PaymentMonth { get; set; }
        public short PaymentYear { get; set; }
        public long? AllowanceConfigId { get; set; }
        [StringLength(100)]
        public string PaymentHeadName { get; set; }
        [StringLength(100)]
        public string PayableHeadName { get; set; }
        public short? PayableMonth { get; set; }
        public short? PayableYear { get; set; }

    }
}
