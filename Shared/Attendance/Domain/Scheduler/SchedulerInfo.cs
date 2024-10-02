using System;
using Shared.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Attendance.Domain.Scheduler
{
    [Table("HR_SchedulerInfo"), Index("ScheduleCode", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_SchedulerInfo_NonClusteredIndex")]
    public class SchedulerInfo : BaseModel3
    {
        [Key]
        public long SchedulerInfoId { get; set; }
        [StringLength(100)]
        public string ScheduleCode { get; set; } // MEET-00000001
        public long HostEmployeeId { get; set; }
        [StringLength(100)]
        public string EmployeeName { get; set; }
        [StringLength(100)]
        public string EmployeeCode { get; set; }
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
        //public long? BranchId { get; set; }
        public ICollection<SchedulerDetail> SchedulerDetails { get; set; }
    }
}
