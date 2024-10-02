using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Attendance.Domain.Attendance
{
    [Table("HR_EmployeeManualAttendance"), Index("ManualAttendanceCode", "EmployeeId", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_EmployeeManualAttendance_NonClusteredIndex")]
    public class EmployeeManualAttendance : BaseModel5
    {
        [Key]
        public long ManualAttendanceId { get; set; }
        [StringLength(100)]
        public string ManualAttendanceCode { get; set; }
        public long EmployeeId { get; set; }
        public long? DepartmentId { get; set; }
        public long? SectionId { get; set; }
        public long? UnitId { get; set; }
        [Column(TypeName = "date")]
        public DateTime? AttendanceDate { get; set; }
        [StringLength(10)]
        public string TimeRequestFor { get; set; } // In / Out / Both
        public TimeSpan? InTime { get; set; }
        public TimeSpan? OutTime { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        [StringLength(200)]
        public string Reason { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
