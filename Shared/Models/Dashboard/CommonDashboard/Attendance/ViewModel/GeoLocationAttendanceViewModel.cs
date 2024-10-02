using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Dashboard.CommonDashboard.Attendance.ViewModel
{
    public class GeoLocationAttendanceViewModel
    {
        public int EmployeeId { get; set; }
        public string Date { get; set; }
        public string PunchIn { get; set; }
        public string PunchOut { get; set; }
        public string AttendanceInLocation { get; set; }
        public string AttendanceOutLocation { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public string AttendanceInRemarks { get; set; }
        public string AttendanceOutRemarks { get; set; }
    }
}
