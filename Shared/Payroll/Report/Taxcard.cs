using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Report
{
    public class TaxCardInfo
    {
        public long TaxProcessId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string LegalName { get; set; }
        public long DesignationId { get; set; }
        public string DesignationName { get; set; }
        public long DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string TINNo { get; set; }
        public long SalaryProcessDetailId { get; set; }
        public long FiscalYearId { get; set; }
        public short SalaryMonth { get; set; }
        public long SalaryYear { get; set; }
        public string SalaryMonthYear { get; set; }
        public decimal? YearlyTaxableIncome { get; set; }
        public decimal? TotalTaxPayable { get; set; }
        public decimal? AITAmount { get; set; }
        public decimal? TaxReturnAmount { get; set; }
        public decimal? ExcessTaxPaidRefundAmount { get; set; }
        public decimal? YearlyTax { get; set; }
        public decimal? PaidTotalTax { get; set; }
        public decimal? MonthlyTax { get; set; }
        public decimal? PFContributionBothPart { get; set; }
        public decimal? OtherInvestment { get; set; }
        public decimal? ActualInvestmentMade { get; set; }
        public decimal? InvestmentRebateAmount { get; set; }
        public decimal? OnceOffTax { get; set; }
        public decimal? ProjectionTax { get; set; }
        public decimal? ArrearAmount { get; set; }
        public decimal? OnceOffAmount { get; set; }
        public decimal? ProjectionAmount { get; set; }
        public string FiscalYearRange { get; set; }
        public string Gender { get; set; }
        public string AssessmentYear { get; set; }
        public long TaxZoneId { get; set; }
        public string TaxZoneName { get; set; }
        public string ChallanNo { get; set; }
        public DateTime? ChallanDate { get; set; }
        public string DepositeBank { get; set; }
        public string DepositeBankBranch { get; set; }
        public string DepositeAmount { get; set; }
        public string ChallanYear { get; set; }
        public string ChallanMonth { get; set; }
        public byte[] AuthorizedSignature { get; set; }
        public long? BranchId { get; set; }
        public string BranchName { get; set; }
        public long CompanyId { get; set; }
        public long OrganizationId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TaxableIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalTillMonthAllowanceAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalCurrentMonthAllowanceAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalProjectedAllowanceAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalLessExemptedAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalGrossAnnualIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? GrossTaxableIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ExemptionAmountOnAnnualIncome { get; set; }
        public string YTD { get; set; }
        public string ProjectedHead { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PFExemption { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalIncomeAfterPFExemption { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ActualTaxDeductionAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? GrossSalary { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TillProjectionTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TillOnceOffTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TillCalculatedTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ActualProjectionTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? AdditionalSubstractAmount { get; set; }

    }
    public class TaxCardDetail
    {
        public long TaxProcessDetailId { get; set; }
        public long TaxProcessId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public long AllowanceHeadId { get; set; }
        public string AllowanceHeadName { get; set; }
        public long AllowanceNameId { get; set; }
        public string AllowanceName { get; set; }
        public string TaxItem { get; set; }
        public decimal? TillDateIncome { get; set; }
        public decimal? CurrentMonthIncome { get; set; }
        public decimal? ProjectedIncome { get; set; }
        public decimal? GrossAnnualIncome { get; set; }
        public decimal? LessExempted { get; set; }
        public decimal? TotalTaxableIncome { get; set; }
        public bool? IsPerquisite { get; set; }
        public string Remarks { get; set; }
        public long? BranchId { get; set; }
        public long CompanyId { get; set; }
        public long OrganizationId { get; set; }
    }
    public class TaxCardSlab
    {
        public long EmployeeTaxProcessSlabId { get; set; }
        public long FiscalYearId { get; set; }
        public string FiscalYearRange { get; set; }
        public long IncomeTaxSlabId { get; set; }
        public string ImpliedCondition { get; set; }
        public decimal? SlabPercentage { get; set; }
        public string ParameterName { get; set; }
        public decimal TaxableIncome { get; set; }
        public decimal TaxLiability { get; set; }
        public long? BranchId { get; set; }
        public long CompanyId { get; set; }
        public long OrganizationId { get; set; }

    }
    public class TaxCardMaster
    {
        public TaxCardMaster()
        {
            TaxCardInfo = new List<TaxCardInfo>();
            TaxCardDetails = new List<TaxCardDetail>();
            TaxCardSlabs = new List<TaxCardSlab>();
        }
        public IEnumerable<TaxCardInfo> TaxCardInfo { get; set; }
        public IEnumerable<TaxCardDetail> TaxCardDetails { get; set; }
        public IEnumerable<TaxCardSlab> TaxCardSlabs { get; set; }
    }
}
