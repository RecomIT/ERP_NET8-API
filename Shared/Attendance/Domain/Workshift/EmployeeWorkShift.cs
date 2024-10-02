using System;
using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Attendance.Domain.Workshift
{
    [Table("HR_EmployeeWorkShifts"), Index("EmployeeId", "StateStatus", "Flag", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_EmployeeWorkShifts_NonClusteredIndex")]
    public class EmployeeWorkShift : BaseModel2
    {
        [Key]
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
        [ForeignKey("WorkShift")]
        public long WorkShiftId { get; set; }
        public WorkShift WorkShift { get; set; }
    }
}
