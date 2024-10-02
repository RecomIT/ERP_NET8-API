using Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shared.Attendance.ViewModel.Workshift
{
    public class WorkShiftViewModel : BaseViewModel3
    {
        public long WorkShiftId { get; set; }
        [Required, StringLength(100)]
        public string Title { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
        [StringLength(200)]
        public string NameDetail { get; set; }
        [StringLength(100)]
        public string NameInBengali { get; set; }
        [StringLength(200)]
        public string NameDetailInBengali { get; set; }
        [Required]
        public TimeSpan? StartTime { get; set; } // 24 Hours
        [Required]
        public TimeSpan? EndTime { get; set; } // 24 Hours
        public short InBufferTime { get; set; }
        [Required]
        public TimeSpan? MaxInTime { get; set; }
        [Required]
        public TimeSpan? LunchStartTime { get; set; }
        [Required]
        public TimeSpan? LunchEndTime { get; set; }
        public TimeSpan? OTStartTime { get; set; }
        public short MaxOTHour { get; set; }
        public short MaxBeforeTime { get; set; }
        public short MaxAfterTime { get; set; }
        public short ExceededMaxAfterTime { get; set; }
        [StringLength(50)]
        public string WeekendDayName { get; set; } // Friday,Saturday
        public List<string> Weekends { get; set; } // To data in WorkShiftWeekend table in json format
        [StringLength(50)]
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
