using System.ComponentModel.DataAnnotations;

namespace Shared.Attendance.DTO.Attendance
{
    public class ManualAttendanceStatusDTO
    {
        [Range(1, long.MaxValue)]
        public long ManualAttendanceId { get; set; }
        public string StateStatus { get; set; }
        public string Remarks { get; set; }
    }
}
