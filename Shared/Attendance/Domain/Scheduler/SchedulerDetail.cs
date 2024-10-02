using System;
using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Attendance.Domain.Scheduler
{
    [Table("HR_SchedulerDetail"), Index("EmployeeId", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_SchedulerDetail_NonClusteredIndex")]
    public class SchedulerDetail : BaseModel1
    {
        [Key]
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
        //public long? BranchId { get; set; }
        [ForeignKey("SchedulerInfo")]
        public long? SchedulerInfoId { get; set; }
        public SchedulerInfo SchedulerInfo { get; set; }
    }
}
