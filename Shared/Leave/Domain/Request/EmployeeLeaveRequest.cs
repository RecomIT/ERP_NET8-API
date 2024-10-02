using System;
using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Leave.Domain.Request
{
    [Table("HR_EmployeeLeaveRequest"), Index("EmployeeLeaveCode", "EmployeeId", "LeaveTypeId", "StateStatus", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_EmployeeLeaveRequest_NonClusteredIndex")]
    public class EmployeeLeaveRequest : BaseModel4
    {
        [Key]
        public long EmployeeLeaveRequestId { get; set; }
        [StringLength(50)]
        public string EmployeeLeaveCode { get; set; }
        public long EmployeeId { get; set; }
        public long? GradeId { get; set; }
        public long? DesignationId { get; set; }
        public long? DepartmentId { get; set; }
        public long? SectionId { get; set; }
        public long? SubSectionId { get; set; }
        public long? UnitId { get; set; }
        public long LeaveTypeId { get; set; }
        [StringLength(100)]
        public string LeaveTypeName { get; set; }
        [StringLength(15)]
        public string DayLeaveType { get; set; } // Full-Day/Half-Day
        [StringLength(20)]
        public string HalfDayType { get; set; } // 1st Half // 2nd Half
        [Column(TypeName = "date")]
        public DateTime? AppliedFromDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? AppliedToDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal AppliedTotalDays { get; set; }
        [StringLength(500)]
        public string LeavePurpose { get; set; }
        [StringLength(100)]
        public string EmergencyPhoneNo { get; set; }
        [StringLength(150)]
        public string AddressDuringLeave { get; set; }
        public long? ReliverId { get; set; }
        public long? ReliverDesignationId { get; set; }
        [StringLength(150)]
        public string AttachmentFileTypes { get; set; } // Comma Separated Values
        public string AttachmentFileNames { get; set; } // Comma Separated Values
        public string AttachmentFiles { get; set; } // Pipe Separated Values |
        [Column(TypeName = "date")]
        public DateTime? ApprovedFromDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ApprovedToDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalApprovalDays { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }
        public long? EmployeeLeaveBalanceId { get; set; }
        public long? SupervisorId { get; set; }
        public long? HODId { get; set; }


        // Leave Files
        public string FileType { get; set; }
        public string FileSize { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string ActualFileName { get; set; }



        // Added By Md. Mahbur Rahman
        [Column(TypeName = "date")]
        public DateTime? EstimatedDeliveryDate { get; set; }
    }
}
