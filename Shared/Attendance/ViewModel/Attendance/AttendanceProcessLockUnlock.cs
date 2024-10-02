using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Attendance.ViewModel.Attendance
{
    public class AttendanceProcessLockUnlock : BaseViewModel2
    {
        [Range(1, long.MaxValue)]
        public long AttendanceProcessId { get; set; }
        [Range(1, short.MaxValue)]
        public short Month { get; set; }
        [Range(1, short.MaxValue)]
        public short Year { get; set; }
        [Required]
        public string IsLocked { get; set; }
    }
}
