using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Attendance.ViewModel.Attendance
{
    public class AttendanceProcessViewModel : BaseViewModel2
    {
        public long AttendanceProcessId { get; set; }
        public short Month { get; set; }
        public short Year { get; set; }
        [StringLength(50)]
        public string MonthYear { get; set; }
        public bool IsLocked { get; set; }
        [StringLength(100)]
        public string LockedBy { get; set; }
        public DateTime? LockedDate { get; set; }
        public string SelectedEmployees { get; set; }
        [Required]
        public DateTime? FromDate { get; set; }
        [Required]
        public DateTime? ToDate { get; set; }
    }
}
