using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.DTO.Payment
{
    public class ConditionalProjectedPaymentApprovalDTO
    {
        [Range(1,long.MaxValue)]
        public long Id { get; set; }
        [Required,StringLength(200)]
        public string Status { get; set; }
    }
}
