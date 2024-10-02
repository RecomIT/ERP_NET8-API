using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Attendance.ViewModel.Workshift
{
    public class EmployeeWorkShiftViewModel : BaseViewModel3
    {
        public long EmployeeWorkShiftId { get; set; }
        [Range(0, long.MaxValue)]
        public long EmployeeId { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        [StringLength(50)]
        public string Flag { get; set; } // JOINING // ROSTER
        public DateTime? ActiveDate { get; set; }
        public DateTime? InActiveDate { get; set; }
        public long? GradeId { get; set; }
        public long? DesignationId { get; set; }
        public long? DepartmentId { get; set; }
        public long? SectionId { get; set; }
        public long? SubsectionId { get; set; }
        public long? UnitId { get; set; }
        public long WorkShiftId { get; set; }
        public long CurrentWorkShiftId { get; set; }

        // Additional
        public string FullName { get; set; }
        public string EmployeeCode { get; set; }
        public string WorkShiftName { get; set; }
        public string GradeName { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public string SectionName { get; set; }
        public string SubSectionName { get; set; }
        public string UnitName { get; set; }
        public string Division { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public string CurrentWorkShiftName { get; set; }
    }
}
