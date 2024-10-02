namespace Shared.Leave.Filter.Request
{
    public class TotalRequestDays_Filter
    {
        public string EmployeeLeaveRequestId { get; set; }
        public string EmployeeId { get; set; }
        public string LeaveTypeId { get; set; }
        public string AppliedFromDate { get; set; }
        public string AppliedToDate { get; set; }
    }
}
