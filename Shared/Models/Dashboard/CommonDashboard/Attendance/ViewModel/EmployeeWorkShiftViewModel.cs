using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Dashboard.CommonDashboard.Attendance.ViewModel
{
    public class EmployeeWorkShiftViewModel
    {
        public int EmployeeId { get; set; }
        public int WorkShiftId { get; set; }
        public int EmployeeWorkShiftId { get; set; }
        public string WorkShiftName { get; set; }
        public string Title { get; set; }
        public string NameDetail { get; set; }
        public TimeSpan StartTime { get; set; } // Change the type to string
        public TimeSpan EndTime { get; set; } // Change the type to string
        public TimeSpan MaxInTime { get; set; }
        public TimeSpan LunchStartTime { get; set; }
        public TimeSpan LunchEndTime { get; set; }
        public string StartHour { get; set; }
        public string StartMin { get; set; }
        public string EndHour { get; set; }
        public string EndMin { get; set; }
    }
}
