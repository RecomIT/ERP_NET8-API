using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Attendance.Domain.Workshift
{
    [Table("HR_WorkShiftWeekends"), Index("DayName", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_WorkShiftWeekends_NonClusteredIndex")]
    public class WorkShiftWeekend : BaseModel
    {
        [Key]
        public long ShiftWeekendId { get; set; }
        public string DayName { get; set; } // Friday, Saturday, Sunday & 4 Others ...
        [StringLength(100)]
        public string WorkShiftName { get; set; }
        [ForeignKey("WorkShift")]
        public long WorkShiftId { get; set; }
        public WorkShift WorkShift { get; set; }
    }
}
