using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Salary
{
    [Table("Payroll_SalaryProcessDetail"), Index("EmployeeId", "FiscalYearId", "SalaryMonth", "SalaryYear", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_SalaryProcessDetail_NonClusteredIndex")]
    public class SalaryProcessDetail : BaseModel1
    {
        [Key]
        public long SalaryProcessDetailId { get; set; }
        public long EmployeeId { get; set; }
        public long? FiscalYearId { get; set; }
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
        public long? JobTypeId { get; set; }
        [StringLength(150)]
        public string JobType { get; set; }
        public long? JobCategoryId { get; set; }
        [StringLength(150)]
        public string JobCategory { get; set; }
        public long? EmployeeTypeId { get; set; }
        [StringLength(150)]
        public string EmployeeType { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> SalaryDate { get; set; }
        public short SalaryMonth { get; set; }
        public short SalaryYear { get; set; }
        [Column(TypeName = "decimal(4,2)")]
        public decimal CalculationForDays { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentBasic { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentHouseRent { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentMedical { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentConveyance { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ThisMonthBasic { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ThisMonthHouseRent { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ThisMonthMedical { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ThisMonthConveyance { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAllowance { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalArrearAllowance { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAllowanceAdjustment { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PFAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PFArrear { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalDeduction { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalArrearDeduction { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ProjectionTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal OnceOffTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalMonthlyTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxDeductedAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalBonus { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal GrossPay { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Payable { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal NetPay { get; set; }
        public long? AccountId { get; set; }
        public long? BankId { get; set; }
        public long? BankBranchId { get; set; }
        [StringLength(50)]
        public string BankAccountNumber { get; set; }
        [StringLength(50)]
        public string WalletAgent { get; set; }
        [StringLength(50)]
        public string WalletNumber { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal WalletTransferAmont { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal COCInWalletTransfer { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal BankTransferAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal CashAmount { get; set; }
        public long? DivisionId { get; set; }
        public bool? IsHoldSalary { get; set; }
        public short? HoldDays { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? HoldAmount { get; set; }
        public short? UnholdDays { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? UnholdAmount { get; set; }
        [StringLength(50)]
        public string SalaryProcessUniqId { get; set; }
        [StringLength(80)]
        public string SalaryReviewInfoIds { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalPFAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ActualPFAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ActualPFArrear { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalActualPFAmount { get; set; }
        [StringLength(200)]
        public string BranchName { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? BreakdownWiseSalaryAdjustment { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> LastSalaryReviewDate { get; set; }
        public long? LastSalaryReviewId { get; set; }
        [ForeignKey("SalaryProcess")]
        public long SalaryProcessId { get; set; }
        public SalaryProcess SalaryProcess { get; set; }
    }
}
