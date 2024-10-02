

namespace Shared.Payroll.Report
{
    public class PayslipInfo
    {
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public long DesignationId { get; set; }
        public string DesignationName { get; set; }
        public long DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public DateTime? DateofBirth { get; set; }
        public DateTime? DateofJoining { get; set; }
        public short WorkingDays { get; set; }
        public string Location { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string BankAccNo { get; set; }
        public string TIN { get; set; }
        public decimal TotalAllowance { get; set; }
        public decimal TotalArrearAllowance { get; set; }
        public decimal TotalDeduction { get; set; }
        public decimal TotalArrearDeduction { get; set; }
        public decimal NetPay { get; set; }
        public long SalaryProcessId { get; set; }
        public short SalaryMonth { get; set; }
        public short SalaryYear { get; set; }
        public DateTime? ProcessDate { get; set; }
        public long? BranchId { get; set; }
        public long CompanyId { get; set; }
        public long OrganizationId { get; set; }
        public decimal BankTransferAmount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? SalaryDate { get; set; }
        public string PaymentMode { get; set; }
        public decimal? PFArrearAmount { get; set; }
        public decimal? PFAmount { get; set; }
        public short PreviousWorkdays { get; set; }
        public long SectionId { get; set; }
        public string SectionName { get; set; }
        public long SubsectionId { get; set; }
        public string SubsectionName { get; set; }
        public short HoldDays { get; set; }
        public decimal? HoldAmount { get; set; }
        public decimal? GrossSalary { get; set; }
        public decimal? ProjectionTaxAmount { get; set; }
        public decimal? OnceOffTaxAmount { get; set; }
        public DateTime? PFActivationDate { get; set; }
        public DateTime? DateOfConfirmation { get; set; }
        public DateTime? LastWorkingDate  { get; set; }
        public bool IsDiscontinued { get; set; } = false;

    }
    public class PayslipAllowance
    {
        public long SalaryAllowanceId { get; set; }
        public long AllowanceNameId { get; set; }
        public string AllowanceName { get; set; }
        public decimal Amount { get; set; }
        //public decimal TotalAmount { get; set; }
    }
    public class PayslipDeduction
    {
        public long SalaryDeductionId { get; set; }
        public long DeductionNameId { get; set; }
        public string DeductionName { get; set; }
        public decimal Amount { get; set; }
        //public decimal TotalAmount { get; set; }
    }
    public class PayslipMaster : PayslipInfo
    {
        public string MonthName { get; set; }
        public string AmountInWord { get; set; }
        public IEnumerable<PayslipDetail> PayslipDetails { get; set; }
    }
    public class PayslipDetail
    {
        public string AllowanceSerial { get; set; }
        public long SalaryAllowanceId { get; set; }
        public long AllowanceNameId { get; set; }
        public string AllowanceName { get; set; }
        public decimal AllowanceAmount { get; set; }
        public decimal AllowanceArrearAmount { get; set; }
        public string DeductionSerial { get; set; }
        public long SalaryDeductionId { get; set; }
        public long DeductionNameId { get; set; }
        public string DeductionName { get; set; }
        public decimal DeductionAmount { get; set; }
        public decimal DeductionArrearAmount { get; set; } // Adjustment
    }
    // Report
}
