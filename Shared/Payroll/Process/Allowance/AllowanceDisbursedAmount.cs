namespace Shared.Payroll.Process.Allowance
{
    public class AllowanceDisbursedAmount
    {
        public long AllowanceNameId { get; set; }
        public string AllowanceName { get; set; }
        public string AllowanceFlag { get; set; }
        public decimal DisbursedAmount { get; set; }
        public decimal Amount { get; set; }
    }
}
