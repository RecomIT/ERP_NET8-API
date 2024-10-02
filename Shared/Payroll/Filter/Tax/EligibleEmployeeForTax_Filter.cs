namespace Shared.Payroll.Filter.Tax
{
    public class EligibleEmployeeForTax_Filter
    {
        public string SelectedEmployees { get; set; }
        public long ProcessDepartmentId { get; set; }
        public long ProcessBranchId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
