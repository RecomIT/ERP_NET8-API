using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Payment
{
    [Table("Payroll_SupplementaryPaymentAmount"), Index("EmployeeId", "PaymentMonth", "PaymentYear", "AllowanceNameId", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_SupplementaryPaymentAmount_NonClusteredIndex")]
    public class SupplementaryPaymentAmount : BaseModel2
    {
        [Key]
        public long PaymentAmountId { get; set; }
        public long? AllowanceHeadId { get; set; }
        public long AllowanceNameId { get; set; }
        public long EmployeeId { get; set; }
        public long? GradeId { get; set; }
        [StringLength(200)]
        public string Grade { get; set; }
        public long? DesignationId { get; set; }
        public long? InternalDesignationId { get; set; }
        [StringLength(200)]
        public string InternalDesignation { get; set; }
        [StringLength(200)]
        public string Designation { get; set; }
        public long? DepartmentId { get; set; }
        [StringLength(200)]
        public string Department { get; set; }
        public long? SectionId { get; set; }
        [StringLength(200)]
        public string Section { get; set; }
        public long? SubSectionId { get; set; }
        [StringLength(200)]
        public string SubSection { get; set; }
        public long? UnitId { get; set; }
        [StringLength(200)]
        public string Unit { get; set; }
        public long? CostCenterId { get; set; }
        [StringLength(200)]
        public string CostCenterName { get; set; }
        [StringLength(200)]
        public string JobType { get; set; }
        public long? JobCategoryId { get; set; }
        [StringLength(200)]
        public string JobCategory { get; set; }
        public long? EmployeeTypeId { get; set; }
        [StringLength(200)]
        public string EmployeeType { get; set; }
        public short PaymentMonth { get; set; }
        public short PaymentYear { get; set; }
        [StringLength(50)]
        public string BaseOfPayment { get; set; } // Flat / Basic / Gross
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Percentage { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? OnceOffAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TaxAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? DisbursedAmount { get; set; }
        public long? FiscalYearId { get; set; }
        public long PaymentProcessInfoId { get; set; }
        [StringLength(100)]
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }

        //Added by Monzur 11-Dec-2023
        public long? EmployeeAccountId { get; set; }
        public long? EmployeeWalletId { get; set; }
        [StringLength(50)]
        public string PaymentMode { get; set; }
        public long? BankId { get; set; }
        [StringLength(150)]
        public string BankName { get; set; }
        public long? BankBranchId { get; set; }
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
        [StringLength(100)]
        public string PaymentHeadName { get; set; }
        [StringLength(100)]
        public string PayableHeadName { get; set; }
        public short? PayableMonth { get; set; }
        public short? PayableYear { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Adjustment { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalAdjustment { get; set; }
    }
}
