namespace Shared.Payroll.DTO.Tax
{
    public class ActualTaxDeductionApprovalDTO
    {
        public short SalaryMonth { get; set; }
        public short SalaryYear { get; set; }
        public string StateStatus { get; set; }
        public List<ActualTaxDeductionApprovalItemsDTO> Employees { get; set; }
    }
}
