
using Microsoft.EntityFrameworkCore;
using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Tax
{
    [Table("Payroll_FinalTaxProcess"), Index("EmployeeId", "FiscalYearId", "SalaryMonth", "SalaryYear", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_FinalTaxProcess_NonClusteredIndex")]
    public class FinalTaxProcess: BaseModel5
    {
        [Key]
        public long TaxProcessId { get; set; }
        public long EmployeeId { get; set; }
        public long? SalaryProcessId { get; set; }
        public long? SalaryProcessDetailId { get; set; }
        public long FiscalYearId { get; set; }
        public short SalaryMonth { get; set; }
        public short RemainMonth { get; set; }
        public short SalaryYear { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalTillMonthAllowanceAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalCurrentMonthAllowanceAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalProjectedAllowanceAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalAdjustmentAmount { get; set; } // Adjustment
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalLessExemptedAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalGrossAnnualIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? GrossTaxableIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ExemptionAmountOnAnnualIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PFContributionBothPart { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? OtherInvestment { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TaxableIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? YearlyTaxableIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalTaxPayable { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? InvestmentRebateAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ActualInvestmentMade { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? AITAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TaxReturnAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ExcessTaxPaidRefundAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? YearlyTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PaidTotalTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal MonthlyTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? SupplementaryOnceffTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ProjectionTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? OnceOffTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ProjectionAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? OnceOffAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ArrearAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PFExemption { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalIncomeAfterPFExemption { get; set; }
        [StringLength(100)]
        public string TaxProcessUniqId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ActualTaxDeductionAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CurrentMonthProjectedTaxDeducted { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CurrentMonthOnceOffTaxDeducted { get; set; }
        public string YTDLabel { get; set; }
        public string ProjectionLabel { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TillProjectionTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TillOnceOffTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ActualProjectionTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? AdditionalSubstractAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? DefaultInvestment { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? DefaultRebate { get; set; }
    }

    [Table("Payroll_FinalTaxProcessDetail"), Index("EmployeeId", "AllowanceNameId", "FiscalYearId", "SalaryMonth", "SalaryYear", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_FinalTaxProcessDetail_NonClusteredIndex")]
    public class FinalTaxProcessDetail: BaseModel1
    {
        [Key]
        public long TaxProcessDetailId { get; set; }
        public long TaxProcessId { get; set; }
        public long EmployeeId { get; set; }
        public long AllowanceHeadId { get; set; }
        [StringLength(200)]
        public string AllowanceHeadName { get; set; }
        public long AllowanceNameId { get; set; }
        [StringLength(200)]
        public string AllowanceName { get; set; }
        public long BonusId { get; set; }
        [StringLength(100)]
        public string BonusName { get; set; }
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
        public long? SalaryProcessDetailId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ArrearAmount { get; set; }
        public bool? IsProjected { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }
        public long? FiscalYearId { get; set; }
        public short SalaryMonth { get; set; }
        public short SalaryYear { get; set; }
        public long? AllowanceConfigId { get; set; }

        // Deduction
        public long? DeductionHeadId { get; set; }
        [StringLength(200)]
        public string DeductionHeadName { get; set; }
        public long? DeductionNameId { get; set; }
        [StringLength(200)]
        public string DeductionNameName { get; set; }
        public long? DeductionConfigId { get; set; }
    }

    [Table("Payroll_FinalTaxProcessSlab"),
       Index("EmployeeId", "FiscalYearId", "SalaryMonth", "SalaryYear", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_FinalTaxProcessSlab_NonClusteredIndex")]
    public class FinalTaxProcessSlab: BaseModel1
    {
        [Key]
        public long EmployeeTaxProcessSlabId { get; set; }
        public long EmployeeId { get; set; }
        public long? FiscalYearId { get; set; }
        public short SalaryMonth { get; set; }
        public short SalaryYear { get; set; }
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
        public Nullable<DateTime> SalaryDate { get; set; }
        public long TaxProcessId { get; set; }
    }
}
