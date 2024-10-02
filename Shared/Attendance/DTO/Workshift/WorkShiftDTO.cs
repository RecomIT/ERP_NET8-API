using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shared.Attendance.DTO.Workshift
{
    public class WorkShiftDTO
    {
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
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public short InBufferTime { get; set; }
        public TimeSpan? MaxInTime { get; set; }
        public TimeSpan? LunchStartTime { get; set; }
        public TimeSpan? LunchEndTime { get; set; }
        public TimeSpan? OTStartTime { get; set; }
        public short MaxOTHour { get; set; }
        public short MaxBeforeTime { get; set; }
        public short MaxAfterTime { get; set; }
        public short ExceededMaxAfterTime { get; set; }
        public List<string> Weekends { get; set; }
        [StringLength(50)]
        public string WeekendDayName { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
    }
}
