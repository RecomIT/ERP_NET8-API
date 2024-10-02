using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Attendance.ViewModel.Scheduler
{
    public class SchedulerDetailViewModel : BaseViewModel1
    {
        public long SchedulerDetailId { get; set; }
        public long EmployeeId { get; set; }
        public long? DepartmentId { get; set; }
        [StringLength(100)]
        public string EmployeeName { get; set; }
        [StringLength(100)]
        public string EmployeeCode { get; set; }
        [StringLength(200)]
        public string Email { get; set; }
        public bool? ParticipantStatus { get; set; } // Agree / Disagree
        [StringLength(200)]
        public string ParticipantRemarks { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ScheduleDate { get; set; }
        public TimeSpan? FromTime { get; set; }
        public TimeSpan? ToTime { get; set; }
        public long? BranchId { get; set; }
        public long? SchedulerInfoId { get; set; }
        // Custom Property
        [StringLength(100)]
        public string DepartmentName { get; set; }
        [StringLength(100)]
        public string BranchName { get; set; }
    }
}
