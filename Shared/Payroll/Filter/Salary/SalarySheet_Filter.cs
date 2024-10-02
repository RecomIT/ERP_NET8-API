namespace Shared.Payroll.Filter.Salary
{
    public class SalarySheet_Filter
    {
        public string EmployeeId { get; set; }
        public string SalaryProcessId { get; set; }
        public string SalaryBatch { get; set; }
        public string SelectedEmployees { get; set; }
        public string SalaryMonth { get; set; }
        public string SalaryYear { get; set; }
        public string DepartmentId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string BranchId { get; set; }
        public string IsDisbursed { get; set; }
    }
}
