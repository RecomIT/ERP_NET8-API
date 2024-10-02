using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.DTO.Payment
{
    public class DeletePaymentDTO
    {
        public long EmployeeId { get; set; } = 0;
        [Range(1, long.MaxValue)]
        public long ProcessId { get; set; } = 0;
        [Range(1, long.MaxValue)]
        public long PaymentId { get; set; } = 0;
        [Range(1,1000000000)]
        public decimal amount { get; set; } = 0;
        public decimal taxAmount { get; set; } = 0;
    }
}
