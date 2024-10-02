namespace Shared.Payroll.Process.Tax
{
    public class DeductableIncomeOfThisEmployee
    {
        public long DeductionNameId { get; set; }
        public string DeductionName { get; set; }
        public string DeductionFlag { get; set; }
        public decimal TillAmount { get; set; } = 0;
        public decimal CurrentAmount { get; set; } = 0;
        public decimal ArrearAmount { get; set; } = 0;
        public bool? IsOnceOff { get; set; }
        public bool? IsProjected { get; set; }
    }
}
