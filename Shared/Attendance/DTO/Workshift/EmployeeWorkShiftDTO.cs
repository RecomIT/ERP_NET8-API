using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Attendance.DTO.Workshift
{
    public class EmployeeWorkShiftDTO
    {
        public long EmployeeWorkShiftId { get; set; }
        public long EmployeeId { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        [StringLength(50)]
        public string Flag { get; set; } // JOINING // ROSTER
        [Column(TypeName = "date")]
        public DateTime? ActiveDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? InActiveDate { get; set; }
        public long? GradeId { get; set; }
        public long? DesignationId { get; set; }
        public long? DepartmentId { get; set; }
        public long? SectionId { get; set; }
        public long? SubsectionId { get; set; }
        public long? UnitId { get; set; }
        //public long? BranchId { get; set; }
        public long CurrentWorkShiftId { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public long WorkShiftId { get; set; }
    }
}
