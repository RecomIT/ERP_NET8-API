using System;
using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Leave.Domain.Balance
{
    [Table("HR_EmployeeLeaveBalance"), Index("EmployeeId", "LeaveTypeId", "LeaveSettingId", "StateStatus", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_EmployeeLeaveBalance_NonClusteredIndex")]
    public class EmployeeLeaveBalance : BaseModel2
    {
        [Key]
        public long EmployeeLeaveBalanceId { get; set; }
        public long EmployeeId { get; set; }
        public long LeaveTypeId { get; set; }
        public long? LeaveSettingId { get; set; }
        [Column(TypeName = "date")]
        public DateTime? LeavePeriodStart { get; set; }
        [Column(TypeName = "date")]
        public DateTime? LeavePeriodEnd { get; set; }
        public short LeaveYear { get; set; }
        [StringLength(100)]
        public string LeaveTypeName { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalLeave { get; set; }



        [Column(TypeName = "decimal(18,2)")]
        public decimal? LeaveApplied { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? LeaveAvailed { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }
        [StringLength(50)]
        public string YearStatus { get; set; }
    }
}
