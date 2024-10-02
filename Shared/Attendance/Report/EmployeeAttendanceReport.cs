using System.Collections.Generic;

namespace Shared.Attendance.Report
{
    public class EmployeeAttendanceReport
    {
        public EmployeeAttendanceReport()
        {
            DailyAttendances = new List<DailyAttendance>();
            AttendanceSummeries = new List<AttendanceSummery>();
            AttendanceWorkShifts = new List<AttendanceWorkShift>();
        }
        public IEnumerable<DailyAttendance> DailyAttendances { get; set; }
        public IEnumerable<AttendanceSummery> AttendanceSummeries { get; set; }
        public List<AttendanceWorkShift> AttendanceWorkShifts { get; set; }
    }
}
