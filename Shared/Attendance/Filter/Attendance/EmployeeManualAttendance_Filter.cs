using Shared.OtherModels.Pagination;

namespace Shared.Attendance.Filter.Attendance
{
    public class EmployeeManualAttendance_Filter : Sortparam
    {
        public string ManualAttendanceId { get; set; }
        public string ManualAttendanceCode { get; set; }
        public string EmployeeId { get; set; }
        public string DepartmentId { get; set; }
        public string Reason { get; set; }
        public string TimeRequestFor { get; set; }
        //public string FromDate { get; set; }
        //public string ToDate { get; set; }
        public string StateStatus { get; set; }
    }
}
