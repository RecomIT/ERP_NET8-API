using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.DTO.Payment
{
    public class SupplementaryProcessMasterDTO
    {
        [Range(1, long.MaxValue)]
        public long AllowanceNameId { get; set; }
        [Range(1, 12)]
        public short PaymentMonth { get; set; }
        [Range(2020, 2050)]
        public short PaymentYear { get; set; }
        [Required]
        public string ProcessType { get; set; }
        public bool ShowInPayslip { get; set; } = false;
        public bool? ShowInSalarySheet { get; set; } = false;
        [Required]
        public string PaymentMode { get; set; }
        public bool WithCOC { get; set; } = false;
        public List<SupplementaryProcessDetailDTO> Details { get; set; }
    }

   
}
