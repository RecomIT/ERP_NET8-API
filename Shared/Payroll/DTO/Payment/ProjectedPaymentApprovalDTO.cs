
using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.DTO.Payment
{
    public class ProjectedPaymentApprovalDTO
    {
        [Range(1,long.MaxValue)]
        public long Id { get; set; }
        [Required,StringLength(50)]
        public string Status { get; set; }
    }
}
