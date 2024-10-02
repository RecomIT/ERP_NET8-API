using System;
using System.ComponentModel.DataAnnotations;


namespace Shared.Leave.DTO.Request
{
    public class ApprovedLeaveCancellationDTO
    {
        [Range(1, long.MaxValue)]
        public long EmployeeLeaveRequestId { get; set; }
        //[Range(1, long.MaxValue)]
        //public long EmployeeId { get; set; }
        //[Range(1, long.MaxValue)]
        public long LeaveTypeId { get; set; }
        [Required, StringLength(200)]
        public string Remarks { get; set; }
    }
}
