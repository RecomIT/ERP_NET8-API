using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Attendance.ViewModel.Attendance
{
    public class AttendanceSummeryViewModel : BaseViewModel2
    {
        public long SummeryId { get; set; }
        public short Year { get; set; }
        public short Month { get; set; }
        public long EmployeeId { get; set; }
        public long? EmployeeTypeId { get; set; }
        public long? DivisionId { get; set; }
        public long? GradeId { get; set; }
        public long? DesignationId { get; set; }
        public long? DepartmentId { get; set; }
        public long? SectionId { get; set; }
        public long? SubSectionId { get; set; }
        public long? UnitId { get; set; }
        public long? WorkShiftId { get; set; }
        [StringLength(40)]
        public string EmployeeCardNo { get; set; }
        [StringLength(20)]
        public string FingerId { get; set; }
        [Column(TypeName = "decimal(2,2)")]
        public decimal? PresentQty { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? AbsentQty { get; set; }
        public short? WeekendQty { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? WorkQtyAtWeekend { get; set; }
        public short? HolidayQty { get; set; }
        public short? WorkQtyAtHoliday { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalLeaveQty { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? FullDayLeaveQty { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? HalfDayLeaveQty { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? WorkQtyAtLeave { get; set; }
        public short? LateQty { get; set; }
        public short? TotalWokingDay { get; set; }
        public TimeSpan? TotalOT { get; set; }
        [StringLength(200)]
        public string AboutLeaveBalance { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }

        // Custom Properties
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DesignationName { get; set; }
        public string MonthName { get; set; }
    }
}
