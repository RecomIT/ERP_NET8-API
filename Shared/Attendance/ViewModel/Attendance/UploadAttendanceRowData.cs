using Microsoft.AspNetCore.Http;
using System;

namespace Shared.Attendance.ViewModel.Attendance
{
    public class UploadAttendanceRowData
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public IFormFile ExcelFile { get; set; }
    }
}
