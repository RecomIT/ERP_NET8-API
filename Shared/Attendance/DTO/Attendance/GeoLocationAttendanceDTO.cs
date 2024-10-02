using System;

namespace Shared.Attendance.DTO.Attendance
{
    public class GeoLocationAttendanceDTO
    {
        public DateTime AttendanceDate { get; set; }
        public TimeSpan AttendanceTime { get; set; }
        public string AttendanceLocation { get; set; }
        public bool IsLocationAttendance { get; set; } = true;
    }
}
