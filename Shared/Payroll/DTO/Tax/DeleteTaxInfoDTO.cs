namespace Shared.Payroll.DTO.Tax
{
    public class DeleteTaxInfoDTO
    {
        public short SalaryMonth { get; set; }
        public short SalaryYear { get; set; }
        public long SalaryProcessId { get; set; }
        public string BatchNo { get; set; }
        public long? EmployeeId { get; set; }
    }
}
