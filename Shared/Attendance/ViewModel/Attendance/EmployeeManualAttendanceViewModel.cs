using Shared.Helpers.ValidationFilters;
using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Attendance.ViewModel.Attendance
{
    public class EmployeeManualAttendanceViewModel : BaseViewModel5
    {
        public long ManualAttendanceId { get; set; }
        [StringLength(100)]
        public string ManualAttendanceCode { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        public long? DepartmentId { get; set; }
        public long? SectionId { get; set; }
        public long? UnitId { get; set; }
        [Required]
        public DateTime? AttendanceDate { get; set; }
        [Required, StringLength(10)]
        public string TimeRequestFor { get; set; } // In / Out / Both
        [RequiredIfValue("TimeRequestFor", new string[] { "In-Time", "Both" })]
        public TimeSpan? InTime { get; set; }
        [RequiredIfValue("TimeRequestFor", new string[] { "Out-Time", "Both" })]
        public TimeSpan? OutTime { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        [Required, StringLength(200)]
        public string Reason { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public long? LineManagerId { get; set; }
        [StringLength(50)]
        public string LineManagerStatus { get; set; }
        [StringLength(200)]
        public string LineManagerRemarks { get; set; }
        public long? SupervisorId { get; set; }
        [StringLength(50)]
        public string SupervisorStatus { get; set; }
        [StringLength(200)]
        public string SupervisorRemarks { get; set; }
        public long? HeadOfDeparmentId { get; set; }
        [StringLength(50)]
        public string HeadOfDeparmentStatus { get; set; }
        [StringLength(200)]
        public string HeadOfDeparmentRemarks { get; set; }
        public long? HRAuthorityId { get; set; }
        [StringLength(50)]
        public string HRAuthorityStatus { get; set; }
        [StringLength(200)]
        public string HRAuthorityRemarks { get; set; }

        // Custom Properties
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string SectionName { get; set; }
        public string UnitName { get; set; }
    }
}
