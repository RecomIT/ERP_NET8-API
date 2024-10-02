using System;
using Shared.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Shared.Attendance.Domain.Workshift
{
    [Table("HR_WorkShifts"), Index("Title", "StateStatus", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_WorkShifts_NonClusteredIndex")]
    public class WorkShift : BaseModel2
    {
        [Key]
        public long WorkShiftId { get; set; }
        [StringLength(100)]
        public string Title { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(200)]
        public string NameDetail { get; set; }
        [StringLength(100)]
        public string NameInBengali { get; set; }
        [StringLength(200)]
        public string NameDetailInBengali { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public TimeSpan? StartTime { get; set; } // 24 Hours
        public TimeSpan? EndTime { get; set; } // 24 Hours
        public short InBufferTime { get; set; }
        public TimeSpan? MaxInTime { get; set; }
        public TimeSpan? LunchStartTime { get; set; }
        public TimeSpan? LunchEndTime { get; set; }
        public TimeSpan? OTStartTime { get; set; }
        public short MaxOTHour { get; set; }
        public short MaxBeforeTime { get; set; }
        public short MaxAfterTime { get; set; }
        public short ExceededMaxAfterTime { get; set; }
        [StringLength(50)]
        public string WeekendDayName { get; set; } //
        [StringLength(50)]
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }
        public ICollection<WorkShiftWeekend> ShiftWeekends { get; set; }
        public ICollection<EmployeeWorkShift> EmployeeWorkShifts { get; set; }
    }
}
