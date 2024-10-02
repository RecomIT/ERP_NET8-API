using Microsoft.EntityFrameworkCore;
using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Payment
{
    [Table("Payroll_ConditionalProjectedPaymentDetail"), Index("EmployeeId", "FiscalYearId", "PaymentMonth", "PaymentYear", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_ConditionalProjectedPaymentDetail_NonClusteredIndex")]
    public class ConditionalProjectedPaymentDetail : BaseModel2
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public long FiscalYearId { get; set; }
        public short PaymentMonth { get; set; }
        public short PaymentYear { get; set; }
        public long? GradeId { get; set; }
        [StringLength(150)]
        public string Grade { get; set; }
        public long? DesignationId { get; set; }
        [StringLength(150)]
        public string Designation { get; set; }
        public long? InternalDesignationId { get; set; }
        [StringLength(150)]
        public string InternalDesignation { get; set; }
        public long? DepartmentId { get; set; }
        [StringLength(150)]
        public string Department { get; set; }
        public long? SectionId { get; set; }
        [StringLength(150)]
        public string Section { get; set; }
        public long? SubsectionId { get; set; }
        [StringLength(150)]
        public string SubSection { get; set; }
        public long? CostCenterId { get; set; }
        [StringLength(150)]
        public string CostCenter { get; set; }
        [StringLength(150)]
        public string CostCenterCode { get; set; }
        public long? UnitId { get; set; }
        [StringLength(150)]
        public string Unit { get; set; }
        [StringLength(150)]
        public string JobType { get; set; }
        public long? JobCategoryId { get; set; }
        [StringLength(150)]
        public string JobCategory { get; set; }
        public long? EmployeeTypeId { get; set; }
        [StringLength(150)]
        public string EmployeeType { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PayableAmount { get; set; } // Basic 50% 10000/=
        [Column(TypeName = "decimal(18,2)")]
        public decimal? DisbursedAmount { get; set; } // 8500/=
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TaxAmount { get; set; } // 8500/=
        public long? BankId { get; set; }
        public long? BankBranchId { get; set; }
        public string AccountNumber { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal BankTransferAmount { get; set; }
        public string WalletAgent { get; set; }
        [StringLength(50)]
        public string WalletNumber { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal WalletTransferAmont { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal COCInWalletTransfer { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal CashAmount { get; set; }
        [StringLength(100)]
        public string StateStatus { get; set; }
        public bool? IsDisbursed { get; set; }
        public bool? ShowInPayslip { get; set; }
        public bool? ShowInSalarySheet { get; set; }
        [ForeignKey("ConditionalProjectedPaymentId")]
        public long ConditionalProjectedPaymentId { get; set; }
        public ConditionalProjectedPayment ConditionalProjectedPayment { get; set; }
    }
}
