using System.ComponentModel.DataAnnotations;

namespace Shared.Leave.DTO.Request
{
    public class DeleteEmployeeLeaveRequestDTO
    {
        [Range(1, long.MaxValue)]
        public long EmployeeLeaveRequestId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [Range(1, long.MaxValue)]
        public long LeaveTypeId { get; set; }
    }
}
