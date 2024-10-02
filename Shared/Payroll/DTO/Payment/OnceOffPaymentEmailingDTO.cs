
using System.ComponentModel.DataAnnotations;
namespace Shared.Payroll.DTO.Payment
{
    public class OnceOffPaymentEmailingDTO
    {
        [Range(1,long.MaxValue)]
        public long ProcessId { get; set; }
        public long PaymentId { get; set; } = 0;
        public long EmployeeId { get; set; } = 0;
        [StringLength(100), Required]
        public string ReportFileName { get; set; } // Payslip /TaxCard / Both
        public bool WithPasswordProtected { get; set; }
        [StringLength(20), Required]
        public string FileFormat { get; set; }// PDF / EXCEL
        public short PaymentMonth { get; set; }
        public short PaymentYear { get; set; }
    }
}
