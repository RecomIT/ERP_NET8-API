using Shared.Models;
using System;

namespace Shared.Attendance.Report
{
    public class AttendanceSummery : BaseViewModel2
    {
        public long SummeryId { get; set; }
        public short Year { get; set; }
        public short Month { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string DesignationName { get; set; }
        public string MonthName { get; set; }
        public short PresentQty { get; set; }
        public short AbsentQty { get; set; }
        public short WeekendQty { get; set; }
        public short WorkQtyAtWeekend { get; set; }
        public short LateQty { get; set; }
        public short FullDayLeaveQty { get; set; }
        public short HalfDayLeaveQty { get; set; }
        public short WorkQtyAtLeave { get; set; }
        public short TotalLeaveQty { get; set; }
        public short HolidayQty { get; set; }
        public short TotalWorkingDay { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public long DesignationId { get; set; }
        public long DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
}
