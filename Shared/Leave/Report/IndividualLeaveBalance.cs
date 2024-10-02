using System;

namespace Shared.Leave.Report
{
    public class IndividualLeaveBalance
    {
        public string LeaveTypeName { get; set; }
        public short TotalLeave { get; set; }
        public short LeaveAvailed { get; set; }
        public short LeaveBalance { get; set; }
        public long EmployeeId { get; set; }
        public DateTime? LeaveStartDate { get; set; }
        public DateTime? LeaveEndDate { get; set; }
    }
}
