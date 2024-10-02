namespace Shared.Payroll.ViewModel.Tax
{
    public class TaxDetailInTaxProcess
    {
        public int SL { get; set; }
        public long EmployeeId { get; set; }
        public long AllowanceNameId { get; set; }
        public long FiscalYearId { get; set; }
        public string AllowanceName { get; set; }
        public bool IsOnceOffTax { get; set; }
        public bool IsProjection { get; set; }
        public string Flag { get; set; }
        public decimal Amount { get; set; }
        public decimal AdjustmentAmount { get; set; }
        public decimal ArrearAmount { get; set; }
        public long AllowanceConfigId { get; set; }
        public decimal TillAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public decimal ProjectedAmount { get; set; }
        public decimal GrossAnnualIncome { get; set; }
        public decimal LessExemptedAmount { get; set; }
        public decimal TotalTaxableIncome { get; set; }
        public decimal ExemptionPercentage { get; set; }
        public decimal ExemptionAmount { get; set; }
    }
}
