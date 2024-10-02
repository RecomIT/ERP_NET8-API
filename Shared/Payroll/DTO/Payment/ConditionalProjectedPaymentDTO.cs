using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.DTO.Payment
{
    public class ConditionalProjectedPaymentDTO
    {
        public long Id { get; set; }
        [Required,Range(1,long.MaxValue)]
        public long FiscalYearId { get; set; }
        [Required, Range(1, long.MaxValue)]
        public long AllowanceNameId { get; set; }
        [Required, StringLength(200)]
        public string Reason { get; set; }
        [Required, Range(2023,2060)]
        public short PayableYear { get; set; }
        [StringLength(200)]
        public string BaseOfPayment { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; } = 0;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Percentage { get; set; } = 0;
        public string JobType { get; set; }
        public string Religion { get; set; }
        public string Citizen { get; set; }
        public bool IsConfirmationRequired { get; set; } = false;
    }
}
