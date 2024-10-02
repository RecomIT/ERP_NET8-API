namespace Shared.Payroll.Filter.Payment
{
    public class FindDepositAllowanceHistory_Filter
    {
        public string EmployeeId { get; set; }
        public string AllowanceNameId { get; set; }
        public string PaymentMonth { get; set; }
        public string PaymentYear { get; set; }
        public string DepositDate { get; set; }
        public string IncomingFlag { get; set; }
        public string ConditionalDepositAllowanceConfigId { get; set; }
        public string EmployeeDepositAllowanceConfigId { get; set; }
    }
}
