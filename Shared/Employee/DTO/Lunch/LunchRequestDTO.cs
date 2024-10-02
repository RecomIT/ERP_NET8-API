using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Employee.DTO.Lunch
{
    public class LunchRequestDTO
    {
        public long LunchRequestId { get; set; }
        public long EmployeeID { get; set; }
        public DateTime LunchDate { get; set; }
        public bool IsLunch { get; set; }
        public DateTime RequestedOn { get; set; }
        public int GuestCount { get; set; }
        public bool IsCanceled { get; set; }
        public int? RequestedByAdminID { get; set; }
    }
}
