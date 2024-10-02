namespace Shared.Leave.Filter.Request
{
    public class LeaveHistory_Filter
    {
        public string EmployeeId { get; set; }
        public string LeaveTypeId { get; set; }
        public string AppliedFromDate { get; set; }
        public string AppliedToDate { get; set; }
        public string StateStatus { get; set; }
    }
}
