using Shared.Models;
using System;

namespace Shared.Attendance.Report
{
    public class DailyAttendance : BaseViewModel2
    {
        public long AttendanceId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string DesignationName { get; set; }
        public long WorkShiftId { get; set; }
        public string WorkShiftName { get; set; }
        public string DayName { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string TotalLateTime { get; set; }
        public string TotalWorkingHours { get; set; }
        public string Status { get; set; }
        public string ShiftStart { get; set; }
        public string ShiftEnd { get; set; }
        public string ShiftTime { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string MonthYear { get; set; }
    }
}
