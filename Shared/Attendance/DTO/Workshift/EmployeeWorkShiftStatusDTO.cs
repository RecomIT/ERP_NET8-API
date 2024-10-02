using System.ComponentModel.DataAnnotations;

namespace Shared.Attendance.DTO.Workshift
{
    public class EmployeeWorkShiftStatusDTO
    {
        public long EmployeeWorkShiftId { get; set; }
        public long EmployeeId { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
    }
}
