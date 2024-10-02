using System;

namespace Shared.Attendance.ViewModel.Workshift
{
    public class EmployeeShiftViewModel
    {
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public long WorkShiftId { get; set; }
        public long EmployeeWorkShiftId { get; set; }
        public string WorkShiftName { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public TimeSpan? MaxInTime { get; set; }
        public TimeSpan? LunchStartTime { get; set; }
        public TimeSpan? LunchEndTime { get; set; }
        public string StartHour { get; set; }
        public string StartMin { get; set; }
        public string EndHour { get; set; }
        public string EndMin { get; set; }
    }
}
