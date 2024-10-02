using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Attendance.Domain.Attendance
{
    [Table("HR_AttendanceProcess"), Index("Month", "Year", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_AttendanceProcess_NonClusteredIndex")]
    public class AttendanceProcess : BaseModel1
    {
        [Key]
        public long AttendanceProcessId { get; set; }
        public short Month { get; set; }
        public short Year { get; set; }
        [StringLength(50)]
        public string MonthYear { get; set; }
        public bool IsLocked { get; set; }
        [StringLength(100)]
        public string LockedBy { get; set; }
        public DateTime? LockedDate { get; set; }
    }
}
