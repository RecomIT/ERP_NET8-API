using System;
using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Attendance.Domain.Attendance
{
    [Table("HR_AttendanceRowData"), Index("EmployeeId", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_AttendanceRowData_NonClusteredIndex")]
    public class AttendanceRowData : BaseModel
    {
        [Key]
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        [StringLength(50)]
        public string EmployeeCode { get; set; }
        public DateTime? AttendanceDate { get; set; }
        [StringLength(50)]
        public string MachineId { get; set; }
    }
}
