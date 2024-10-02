namespace Shared.Payroll.Process.Tax
{
    public class AllowancesEarnedByThisEmployee
    {
        public long AllowanceHeadId { get; set; }
        public long AllowanceNameId { get; set; }
        public string AllowanceName { get; set; }
        public string AllowanceFlag { get; set; }
        public decimal TillAmount { get; set; } = 0;
        public decimal TillAdjustment { get; set; } = 0;
        public decimal CurrentAmount { get; set; } = 0;
        public decimal ArrearAmount { get; set; } = 0;
        public decimal CurrentAdjustment { get; set; } = 0;
        public bool? IsOnceOff { get; set; }
        public bool? IsProjected { get; set; }
    }
}
