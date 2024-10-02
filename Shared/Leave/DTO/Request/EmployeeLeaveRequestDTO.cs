using Microsoft.AspNetCore.Http;
using Shared.Helpers.ValidationFilters;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Leave.DTO.Request
{
    public class EmployeeLeaveRequestDTO
    {
        public long EmployeeLeaveRequestId { get; set; }
        [StringLength(50)]
        public string EmployeeLeaveCode { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
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
        [StringLength(200)]
        public string Remarks { get; set; }
        public string LeaveDaysJson { get; set; }
        [AllowedExtensions(new string[] { ".pdf", ".jpeg", ".jpg", ".png" })]
        public IFormFile File { get; set; }
        public string FilePath { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EstimatedDeliveryDate { get; set; }
    }
}
