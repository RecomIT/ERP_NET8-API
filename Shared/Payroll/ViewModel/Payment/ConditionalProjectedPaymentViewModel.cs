namespace Shared.Payroll.ViewModel.Payment
{
    public class ConditionalProjectedPaymentViewModel
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string AllowanceName { get; set; }
        public string BaseOfPayment { get; set; }
        public decimal Amount { get; set; }
        public decimal Percentage { get; set; }
        public string FiscalYear { get; set; }
        public string Jobtype { get; set; }
        public string Citizen { get; set; }
        public string Religion { get; set; }
        public string IsConfirmationRequired { get; set; }
        public string Gender { get; set; }
        public short PaymentMonth { get; set; }
        public short PaymentYear { get; set; }
        public short PayableMonth { get; set; }
        public short PayableYear { get; set; }
        public decimal PayableAmount { get; set; }
        public decimal DisbursedAmount { get; set; }
        public string StateStatus { get; set; }
        public int Count { get; set; } = 0;
        public string Reason { get; set; }
    }
}
