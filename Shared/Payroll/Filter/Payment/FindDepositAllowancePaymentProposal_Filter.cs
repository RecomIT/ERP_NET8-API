namespace Shared.Payroll.Filter.Payment
{
    public class FindDepositAllowancePaymentProposal_Filter
    {
        public string EmployeeId { get; set; }
        public string AllowanceNameId { get; set; }
        public string PaymentBeMade { get; set; }
        public string PaymentDate { get; set; }
        public string PaymentMonth { get; set; }
        public string PaymentYear { get; set; }
        public string IncomingFlag { get; set; }
        public string ConditionalDepositAllowanceConfigId { get; set; }
        public string EmployeeDepositAllowanceConfigId { get; set; }
    }
}
