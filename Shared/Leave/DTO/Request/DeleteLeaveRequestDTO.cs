namespace Shared.Leave.DTO.Request
{
    public class DeleteLeaveRequestDTO
    {
        public long? LeaveRequestId { get; set; }
        public long? EmployeeId { get; set; }
        public long? LeaveTypeId { get; set; }
    }
}
