namespace Shared.Payroll.ViewModel.Salary
{
    public class EmployeeLastApprovedSalaryReviewDetail
    {
        public long SalaryReviewDetailId { get; set; }
        public long AllowanceNameId { get; set; }
        public string AllowanceName { get; set; }
        public string AllowanceBase { get; set; }
        public decimal CurrentAmount { get; set; }
        public decimal PreviousAmount { get; set; }
        public decimal AdditionalAmount { get; set; }
        public string AllowanceFlag { get; set; }
        public long SalaryAllowanceConfigDetailId { get; set; }
    }
}
