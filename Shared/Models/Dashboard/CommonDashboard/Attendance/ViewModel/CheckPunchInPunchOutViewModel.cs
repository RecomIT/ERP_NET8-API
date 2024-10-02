using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Dashboard.CommonDashboard.Attendance.ViewModel
{
    public class CheckPunchInPunchOutViewModel
    {
        public bool PunchIn { get; set; }
        public bool PunchOut { get; set; }
        public bool PunchInAndPunchOut { get; set; }
        public TimeSpan ShiftInTime { get; set; }
        public TimeSpan MaxInTime { get; set; }
        public TimeSpan ActualInTime { get; set; }
        public TimeSpan EarlyInTime { get; set; } 
        public TimeSpan LateInTime { get; set; } 

        public TimeSpan ShiftEndTime { get; set; }
        public TimeSpan ActualOutTime { get; set; }
        public TimeSpan EarlyGoing { get; set; }
        public TimeSpan OverTime { get; set; }

        public string InTimeLocation { get; set; }
        public string OutTimeLocation { get; set; }

    }
}
