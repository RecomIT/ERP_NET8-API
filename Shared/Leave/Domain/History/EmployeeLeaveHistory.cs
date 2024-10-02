using System;
using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Leave.Domain.History
{
    [Table("HR_EmployeeLeaveHistory"), Index("LeaveHistoryId", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_EmployeeLeaveHistory_NonClusteredIndex")]
    public class EmployeeLeaveHistory : BaseModel4
    {
        [Key]
        public long LeaveHistoryId { get; set; }
        public long EmployeeId { get; set; }
        public long? DesignationId { get; set; }
        public long? DepartmentId { get; set; }
        [Column(TypeName = "decimal(4,2)")]
        public decimal Count { get; set; }
        public long? WorkShiftId { get; set; }
        public long? LeaveTypeId { get; set; }
        public long? LeaveSettingId { get; set; }
        [StringLength(50)]
        public string Status { get; set; } // Rollback/Enjoyed/Availed
        [Column(TypeName = "date")]
        public DateTime? LeaveDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ReplacementDate { get; set; }
        public long? EmployeeLeaveRequestId { get; set; }
        public long? EmployeeLeaveBalanceId { get; set; }
    }
}
