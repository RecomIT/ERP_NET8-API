using Shared.Helpers.ValidationFilters;
using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Leave.ViewModel.Request
{
    public class EmployeeLeaveRequestViewModel : BaseViewModel5
    {
        public long EmployeeLeaveRequestId { get; set; }
        [StringLength(50)]
        public string EmployeeLeaveCode { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        public long? GradeId { get; set; }
        public long? DesignationId { get; set; }
        public long? DepartmentId { get; set; }
        public long? SectionId { get; set; }
        public long? SubSectionId { get; set; }
        public long? UnitId { get; set; }
        [Range(1, long.MaxValue)]
        public long LeaveTypeId { get; set; }
        [StringLength(100)]
        public string LeaveTypeName { get; set; }
        [StringLength(15)]
        public string DayLeaveType { get; set; } // Full-Day/Half-Day
        [RequiredIfValue("DayLeaveType", new string[] { "Half-Day" }), StringLength(20)]
        public string HalfDayType { get; set; } // First Portion // Second Portion
        [Required, Column(TypeName = "date")]
        public DateTime? AppliedFromDate { get; set; }
        [RequiredIfValue("DayLeaveType", new string[] { "Full-Day" }), Column(TypeName = "date")]
        public DateTime? AppliedToDate { get; set; }
        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal AppliedTotalDays { get; set; }
        [Required, StringLength(500)]
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
        [Column(TypeName = "date")]
        public DateTime? EstimatedDeliveryDate { get; set; }

        // Custom Properties
        public string EmployeeName { get; set; }
        public string GradeName { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public string SectionName { get; set; }
        public string SubSectionName { get; set; }
        public string UnitName { get; set; }
        public string ReliverName { get; set; }
        public string ReliverDesignationName { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime? LeaveDate { get; set; }
        public decimal? LeaveBalance { get; set; }
        public string CreaterInfo { get; set; }
        public string UpdaterInfo { get; set; }
        public string ApproverInfo { get; set; }
        public string RejecterInfo { get; set; }
        public string CheckerInfo { get; set; }
        public string CancellerInfo { get; set; }
        public long SupervisorId { get; set; }
        public string SupervisorName { get; set; }
        public long HODId { get; set; }
        public string HODName { get; set; }
        public string FileType { get; set; }
        public string FileSize { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string ActualFileName { get; set; }
    }
}
