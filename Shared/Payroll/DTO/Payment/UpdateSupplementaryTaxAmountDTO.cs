namespace Shared.Payroll.DTO.Payment
{
    public class UpdateSupplementaryTaxAmountDTO
    {
        public long PaymentProcessInfoId { get; set; }
        public long EmployeeId { get; set; }
        public decimal TaxAmount { get; set; }
    }
}
