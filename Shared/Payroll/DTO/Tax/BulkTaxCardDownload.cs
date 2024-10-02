namespace Shared.Payroll.DTO.Tax
{
    public class BulkTaxCardDownload
    {
        public long SalaryProcessId { get; set; }
        public short Month { get; set; }
        public short Year { get; set; }
    }
}
