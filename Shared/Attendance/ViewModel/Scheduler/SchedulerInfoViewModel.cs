using Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Attendance.ViewModel.Scheduler
{
    public class SchedulerInfoViewModel : BaseViewModel4
    {
        public long SchedulerInfoId { get; set; }
        [StringLength(100)]
        public string ScheduleCode { get; set; } // MEET-00000001
        public long HostEmployeeId { get; set; }
        [StringLength(100)]
        public string EmployeeName { get; set; }

        [StringLength(200)]
        public string Email { get; set; }
        public long? DepartmentId { get; set; }
        [StringLength(200)]
        public string Subject { get; set; }
        [StringLength(300)]
        public string Details { get; set; }
        [StringLength(200)]
        public string Location { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ScheduleDate { get; set; }
        public TimeSpan? FromTime { get; set; }
        public TimeSpan? ToTime { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
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
        // Custom property
        public short GuestCount { get; set; }
        [StringLength(100)]
        public string EmployeeCode { get; set; }
        public string DepartmentName { get; set; }
        public string LineManagerName { get; set; }
        public string SupervisorName { get; set; }
        public string HRAuthorityName { get; set; }
        public string HeadOfDepartmentName { get; set; }
        public List<SchedulerDetailViewModel> SchedulerDetails { get; set; }
    }
}
