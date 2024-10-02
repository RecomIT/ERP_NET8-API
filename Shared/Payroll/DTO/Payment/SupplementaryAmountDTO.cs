

using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.DTO.Payment
{
    public class SupplementaryAmountDTO
    {
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [Range(1, 1000000000)]
        public decimal Amount { get; set; }
        public long AllowanceHeadId { get; set; } = 0;
        public long AllowanceNameId { get; set; } = 0;
        public short PaymentMonth { get; set; } = 0;
        public short PaymentYear { get; set; } = 0;
        public string PaymentMode { get; set; }
    }
}
