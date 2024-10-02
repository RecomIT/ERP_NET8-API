using System;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Attendance.Domain.Attendance
{
    [Table("HR_GeoLocationAttendance")]
    public class GeoLocationAttendance : BaseModel
    {
        [Key]
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public DateTime? AttendanceInTime { get; set; }
        public DateTime? AttendanceOutTime { get; set; }
        public string AttendanceInLocation { get; set; }
        public string AttendanceOutLocation { get; set; }
        public string AttendanceInRemarks { get; set; }
        public string AttendanceOutRemarks { get; set; }
        public string AttendanceType { get; set; }
    }
}
