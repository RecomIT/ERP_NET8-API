using Shared.Models;

namespace Shared.Attendance.Report
{
    public class AttendanceWorkShift : BaseViewModel2
    {
        public long WorkShiftId { get; set; }
        public string WorkShiftName { get; set; }
        public int TotalDaysInShift { get; set; }
        public string ShiftStart { get; set; }
        public string ShiftEnd { get; set; }
        public string ShiftTime { get; set; }
    }
}
