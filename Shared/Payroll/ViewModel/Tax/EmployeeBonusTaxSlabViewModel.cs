namespace Shared.Payroll.ViewModel.Tax
{
    public class EmployeeBonusTaxSlabViewModel
    {
        public long EmployeeId { get; set; }
        public long FiscalYearId { get; set; }
        public long IncomeTaxSlabId { get; set; }
        public string ImpliedCondition { get; set; }
        public decimal SlabPercentage { get; set; }
        public string ParameterName { get; set; }
        public decimal TaxableIncome { get; set; }
        public decimal TaxLiablity { get; set; }
    }
}

