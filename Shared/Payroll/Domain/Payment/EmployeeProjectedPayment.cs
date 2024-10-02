using Microsoft.EntityFrameworkCore;
using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Payment
{
    [Table("Payroll_EmployeeProjectedAllowance"), Index("EmployeeId", "FiscalYearId", "PaymentMonth", "PaymentYear", "AllowanceNameId", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_EmployeeProjectedAllowance_NonClusteredIndex")]
    public class EmployeeProjectedPayment : BaseModel2
    {
        [Key]
        public long ProjectedAllowanceId { get; set; }
        [StringLength(100)]
        public string ProjectedAllowanceCode { get; set; } // PA-0000001
        public long EmployeeId { get; set; }
        public long? FiscalYearId { get; set; }
        public short? PaymentMonth { get; set; }
        public short? PaymentYear { get; set; }
        public long? AllowanceHeadId { get; set; }
        public long AllowanceNameId { get; set; }
        [StringLength(50)]
        public string BaseOfPayment { get; set; } // Flat(5000/=) / Basic (50%) / Gross
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Percentage { get; set; } // Basic/Gross = 100%
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; } // 5000/=
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PayableAmount { get; set; } // Basic 50% 10000/=
        [Column(TypeName = "decimal(18,2)")]
        public decimal? DisbursedAmount { get; set; } // 8500/=
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TaxAmount { get; set; } // 8500/=
        [StringLength(100)]
        public string StateStatus { get; set; } // Pending / Approved / Processed / Disbursed
        public bool IsApproved { get; set; }
        public bool? IsDisbursed { get; set; }
        public bool? ShowInPayslip { get; set; }
        public bool? ShowInSalarySheet { get; set; }
        [StringLength(150)]
        public string AllowanceReason { get; set; }
        //Added by Monzur 04-Jan-2024
        public long? EmployeeAccountId { get; set; }
        public long? EmployeeWalletId { get; set; }
        [StringLength(50)]
        public string PaymentMode { get; set; }
        [StringLength(150)]
        public string BankName { get; set; }
        [StringLength(150)]
        public string BankBranchName { get; set; }
        [StringLength(150)]
        public string RoutingNumber { get; set; }
        [StringLength(100)]
        public string BankAccountNumber { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? BankTransferAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? WalletTransferAmount { get; set; }
        public bool? WithCOC { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? COCInWalletTransfer { get; set; }
        [StringLength(50)]
        public string WalletNumber { get; set; }
        [StringLength(50)]
        public string WalletAgent { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CashAmount { get; set; }
        [StringLength(100)]
        public string PayrollCardNumber { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public short? PayableMonth { get; set; }
        public short? PayableYear { get; set; }
        [StringLength(100)]
        public string PaymentHeadName { get; set; }
        [StringLength(100)]
        public string PayableHeadName { get; set; }
        [ForeignKey("EmployeeProjectedAllowanceProcessInfo")]
        public long? ProjectedAllowanceProcessInfoId { get; set; }
        public EmployeeProjectedAllowanceProcessInfo EmployeeProjectedAllowanceProcessInfo { get; set; }
    }
}
