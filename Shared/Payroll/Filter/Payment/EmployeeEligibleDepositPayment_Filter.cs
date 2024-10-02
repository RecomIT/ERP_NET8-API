namespace Shared.Payroll.Filter.Payment
{
    public class EmployeeEligibleDepositPayment_Filter
    {
        public string ConfigId { get; set; }
        public string EmployeeId { get; set; }
        public string BranchId { get; set; }
        public string HasPayableAmount { get; set; } = "Yes";
    }
}
