namespace Shared.Payroll.DTO.Tax
{
    public class FinalTaxProcessDTO
    {
        public List<long> EmployeeIds { get; set; }
        public long FiscalYearId { get; set; }
        public string Flag { get; set; }
    }
}
