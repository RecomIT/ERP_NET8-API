using System;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.ViewModel.Salary
{
    public class SalaryProcessDetailViewModel : BaseViewModel2
    {
        public long SalaryProcessDetailId { get; set; }
        public string BatchNo { get; set; }
        public long EmployeeId { get; set; }
        public long? DesignationId { get; set; }
        public long? DepartmentId { get; set; }
        public long? SectionId { get; set; }
        public long? SubsectionId { get; set; }
        public long? UnitId { get; set; }
        public long? JobTypeId { get; set; }
        public long? FiscalYearId { get; set; }
        public long? GradeId { get; set; }
        [StringLength(150)]
        public string Grade { get; set; }
        [StringLength(150)]
        public string Designation { get; set; }
        public long? InternalDesignationId { get; set; }
        [StringLength(150)]
        public string InternalDesignation { get; set; }
        [StringLength(150)]
        public string Department { get; set; }
        [StringLength(150)]
        public string Section { get; set; }
        [StringLength(150)]
        public string SubSection { get; set; }
        public long? CostCenterId { get; set; }
        [StringLength(150)]
        public string CostCenter { get; set; }
        [StringLength(150)]
        public string CostCenterCode { get; set; }
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
        public long? BankId { get; set; }
        public long? BankBranchId { get; set; }
        public long? AccountId { get; set; }
        public bool IsDisbursed { get; set; }
        public short SalaryMonth { get; set; }
        public short SalaryYear { get; set; }
        [Column(TypeName = "date")]
        public DateTime? SalaryDate { get; set; }
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
        public decimal TotalAllowance { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalArrearAllowance { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PFAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PFArrear { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal OtherDeduction { get; set; }
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
        public decimal TotalBonus { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAllowanceAdjustment { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal GrossPay { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal NetPay { get; set; }
        public long? DivisionId { get; set; }
        public long SalaryProcessId { get; set; }
        public short? HoldDays { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? HoldAmount { get; set; }
        public short? UnholdDays { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? UnholdAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TaxDeductedAmount { get; set; }

        // Custom Properties
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string GradeName { get; set; }
        public string DesignationName { get; set; }
        public string SectionName { get; set; }
        public string SubSectionName { get; set; }
        public string UnitName { get; set; }
        public string JobTypeName { get; set; }
        public string DivisionName { get; set; }
        public string BankName { get; set; }
        public string BankBranchName { get; set; }
        public string AccountNo { get; set; }
        public string Email { get; set; }
        public string PersonalEmail { get; set; }
    }
}
