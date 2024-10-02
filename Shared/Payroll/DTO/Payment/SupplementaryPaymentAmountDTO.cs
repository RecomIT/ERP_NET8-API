using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.DTO.Payment
{
    public class SupplementaryPaymentAmountDTO
    {
        public long PaymentAmountId { get; set; }
        [Range(1, 12)]
        public short PaymentMonth { get; set; }
        [Range(2022, 2050)]
        public short PaymentYear { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        public long? FiscalYearId { get; set; }
        public string EmployeeCode { get; set; }
        [Range(1, long.MaxValue)]
        public long AllowanceNameId { get; set; }
        [StringLength(50), Required]
        public string BaseOfPayment { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Percentage { get; set; }
    }
}
