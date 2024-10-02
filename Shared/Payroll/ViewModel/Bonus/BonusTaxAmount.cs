namespace Shared.Payroll.ViewModel.Bonus
{
    public class BonusTaxAmount
    {
        public decimal TaxAmount { get; set; }
        public decimal ProjectionTax { get; set; }
        public decimal OnceOffTax { get; set; }
        public short RemainMonth { get; set; }
    }
}
