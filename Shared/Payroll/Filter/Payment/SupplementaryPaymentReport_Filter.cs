using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.Filter.Payment
{
    public class SupplementaryPaymentReport_Filter
    {
        [Range(1, long.MaxValue)]
        public long ProcessId { get; set; }
        [Range(1, long.MaxValue)]
        public long PaymentId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [Range(1, short.MaxValue)]
        public short PaymentMonth { get; set; }
        [Range(1, short.MaxValue)]
        public short PaymentYear { get; set; }
        public bool? IsDisbursed { get; set; }
    }
}
