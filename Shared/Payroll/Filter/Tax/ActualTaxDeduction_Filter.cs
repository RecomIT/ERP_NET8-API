namespace Shared.Payroll.Filter.Tax
{
    public class ActualTaxDeduction_Filter
    {
        public string ActualTaxDeductionId { get; set; }
        public string EmployeeId { get; set; }
        public string FiscalYearId { get; set; }
        public string SalaryMonth { get; set; }
        public string SalaryYear { get; set; }
        public string StateStatus { get; set; }
    }
}
