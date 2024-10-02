using Shared.OtherModels.Pagination;

namespace Shared.Leave.Filter.Request
{
    public class LeaveRequest_Filter : Sortparam
    {
        public string EmployeeLeaveRequestId { get; set; }
        public string EmployeeId { get; set; }
        public string DepartmentId { get; set; }
        public string DesignationId { get; set; }
        public string SectionId { get; set; }
        public string SubsectionId { get; set; }
        public string LeaveTypeId { get; set; }
        public string DayLeaveType { get; set; }
        public string StateStatus { get; set; }
        public string SupervisorId { get; set; }
        public string AppliedFromDate { get; set; }
        public string AppliedToDate { get; set; }
    }
}
