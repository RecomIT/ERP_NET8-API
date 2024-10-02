using System.ComponentModel.DataAnnotations;

namespace Shared.Leave.DTO.Request
{
    public class EmployeeLeaveRequestStatusDTO
    {
        public long EmployeeId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeLeaveRequestId { get; set; }
        public string Remarks { get; set; }
        [StringLength(50), Required]
        public string StateStatus { get; set; }
    }
}
