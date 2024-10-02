namespace Shared.Attendance.Filter.Report
{
    public class DailyAttendanceReport_Filter
    {
        public string EmployeeId { get; set; }
        public string SelectedEmployees { get; set; }
        public string AttendanceMonth { get; set; }
        public string AttendanceYear { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
