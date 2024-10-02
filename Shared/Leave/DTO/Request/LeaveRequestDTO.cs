using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shared.Leave.DTO.Request
{
    public class LeaveRequestDTO
    {
        public EmployeeLeaveRequestDTO LeaveRequest { get; set; }
        public List<EmployeeLeaveDaysDTO> LeaveDays { get; set; }


        // File
        public IFormFile File { get; set; }
        public string FileSize { get; set; }
        //[StringLength(300)]
        public string FilePath { get; set; }

        [StringLength(300)]
        public string ExistsFilePath { get; set; }

        [StringLength(100)]
        public string FileType { get; set; }

        [StringLength(200)]
        public string FileName { get; set; }

        [StringLength(200)]
        public string ActualFileName { get; set; }
        [StringLength(200)]
        public string ExistsFileName { get; set; }
    }
}
