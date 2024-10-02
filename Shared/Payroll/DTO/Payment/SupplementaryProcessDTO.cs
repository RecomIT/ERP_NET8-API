using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.DTO.Payment
{
    public class SupplementaryProcessDTO
    {
        [Range(1, 12)]
        public short PaymentMonth { get; set; }
        [Range(2020, 2050)]
        public short PaymentYear { get; set; }
        public string PaymentAmountIds { get; set; }
        public string ProcessType { get; set; }
        [Range(1, long.MaxValue)]
        public long AllowanceNameId { get; set; }
        public string AllowanceName { get; set; }
        public bool? ShowInPayslip { get; set; }
        public bool? ShowInSalarySheet { get; set; }
        [Required]
        public string PaymentMode { get; set; }
        public bool? WithCOC { get; set; }
        public List<SupplementaryAmountDTO> Payments { get; set; }
    }
}
