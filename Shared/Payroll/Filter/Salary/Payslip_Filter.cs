namespace Shared.Payroll.Filter.Salary
{
    public class Payslip_Filter
    {
        public string EmployeeId { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Format { get; set; }
        public string IsDisbursed { get; set; }
    }
}
