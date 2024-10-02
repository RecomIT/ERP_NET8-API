namespace Shared.Payroll.Filter.Salary
{
    public class EligibleEmployeeForSalary_Filter
    {
        public string SelectedEmployees { get; set; }
        public string SelectedDepartmentId { get; set; }
        public string SelectedBranchId { get; set; }
        public string SalaryDate { get; set; }
        public string SalaryStartDate { get; set; }
        public int SalaryMonth { get; set; }
        public int SalaryYear { get; set; }
    }
}
