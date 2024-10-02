using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Control_Panel.DTO.GoogleAuthenticator
{
    public class EmployeeDTO
    {
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; } = "-";
        public string Name { get; set; } = "-"; public string Designation { get; set; } = "-";
        public string Department { get; set; } = "-"; public string Division { get; set; } = "-";
        public string Branch { get; set; } = "-"; public string OfficeEmail { get; set; }
        public string SupervisorEmail { get; set; }
        public string SupervisorName { get; set; }
    }
}