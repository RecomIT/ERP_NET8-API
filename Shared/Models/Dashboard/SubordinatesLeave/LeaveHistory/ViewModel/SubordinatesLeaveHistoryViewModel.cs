using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Dashboard.SubordinatesLeave.LeaveHistory.ViewModel
{
    public class SubordinatesLeaveHistoryViewModel
    {
        public long EmployeeId { get; set; }
        public string Name { get; set; }
        public long LeaveTypeId { get; set; }
        public string Title { get; set; }
        public string ShortName { get; set; }
        public short LeaveCount { get; set; }
        public string LeaveDates { get; set; }
        public short TotalEmployees { get; set; }
        public short GrandTotalLeaveCount { get; set; }
        public short TotalLeaveCountPerEmployee { get; set; }
        public string PhotoPath { get; set; }
        public string Photo { get; set; }

    }
}
