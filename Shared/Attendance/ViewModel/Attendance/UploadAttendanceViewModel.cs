using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Attendance.ViewModel.Attendance
{
    public class UploadAttendanceViewModel
    {
        [StringLength(50)]
        public string EmployeeCode { get; set; }
        public DateTime? AttendanceDate { get; set; }
        [StringLength(50)]
        public string MachineId { get; set; }
    }
}
