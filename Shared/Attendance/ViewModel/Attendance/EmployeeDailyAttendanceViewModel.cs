using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Attendance.ViewModel.Attendance
{
    public class EmployeeDailyAttendanceViewModel : BaseViewModel2
    {
        public long AttendanceId { get; set; }
        public long EmployeeId { get; set; }
        public long? EmployeeTypeId { get; set; }
        public long? DivisionId { get; set; }
        public long? GradeId { get; set; }
        public long? DesiginationId { get; set; }
        public long? DepartmentId { get; set; }
        public long? SectionId { get; set; }
        public long? SubSectionId { get; set; }
        public long? UnitId { get; set; }
        public long WorkShiftId { get; set; }
        [StringLength(50)]
        public string EmployeeCardNo { get; set; }
        [StringLength(30)]
        public string FingerId { get; set; }
        [Column(TypeName = "date")]
        public DateTime? TransactionDate { get; set; }
        public TimeSpan? InTime { get; set; }
        public TimeSpan? OutTime { get; set; }
        public short? InHour { get; set; }
        public short? InMinute { get; set; }
        public short? OutHour { get; set; }
        public short? OutMinute { get; set; }
        public TimeSpan? TotalLateTime { get; set; }
        public TimeSpan? OT { get; set; }
        public short? OTHours { get; set; }
        public short? OTMinutes { get; set; }
        public bool IsFromMachine { get; set; }
        public TimeSpan? TotalWorkingHours { get; set; }
        [StringLength(20)]
        public string InTimeMachineNumber { get; set; }
        [StringLength(20)]
        public string OutTimeMachineNumber { get; set; }
        [StringLength(50)]
        public string Status { get; set; } // Present / Late / Leave / Absent / Short-Leave
        [StringLength(100)]
        public string YearlyHolidayId { get; set; }
        [StringLength(100)]
        public string EmployeeLeaveReqId { get; set; }
        [StringLength(100)]
        public string LeaveTypeId { get; set; }
        [StringLength(100)]
        public string ManualReqId { get; set; }
        [StringLength(100)]
        public string SchedulerInfoId { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public bool? IsWorkingDay { get; set; }
        public bool? IsWeekend { get; set; }
        public bool? IsHoliday { get; set; }
        public bool? IsLeaveDay { get; set; }
        public bool? IsPresent { get; set; }
        public bool? IsLate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? LeaveQty { get; set; }

        // Custom Properties
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string LeaveTypeName { get; set; }
        public string HolidayName { get; set; }
        public string DayName { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public TimeSpan? MaxInTime { get; set; }
    }
}
