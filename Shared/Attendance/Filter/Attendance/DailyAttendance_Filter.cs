using Shared.OtherModels.Pagination;

namespace Shared.Attendance.Filter.Attendance
{
    public class DailyAttendance_Filter : Sortparam
    {
        public string EmployeeId { get; set; }
        public string DesignationId { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string AttStatus { get; set; }
    }
}
