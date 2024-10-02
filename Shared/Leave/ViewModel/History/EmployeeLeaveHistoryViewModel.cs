using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Leave.ViewModel.History
{
    public class EmployeeLeaveHistoryViewModel : BaseViewModel2
    {
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
        public string Status { get; set; } // Rollback/Enjoyed
        public DateTime? LeaveDate { get; set; }
        // Custom Properties
        public string EmployeeName { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public string WorkShiftName { get; set; }
        public string LeaveTypeName { get; set; }
    }
}
