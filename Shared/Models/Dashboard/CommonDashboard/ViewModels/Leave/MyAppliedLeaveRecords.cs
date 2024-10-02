using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Dashboard.CommonDashboard.ViewModels.Leave
{
    public class MyAppliedLeaveRecords
    {
        public short TotalRowCount { get; set; }

        public long EmployeeLeaveRequestId { get; set; }
        public long EmployeeId { get; set; }
        public string Name { get; set; }
        public string DesignationName { get; set; }
        public long LeaveTypeId { get; set; }
        public string Title { get; set; }
        public short LeaveCount { get; set; }
        public DateTime AppliedFromDate { get; set; }
        public DateTime AppliedToDate { get; set; }
        public short AppliedTotalDays { get; set; }
        public short TotalEmployees { get; set; }
        public string StateStatus { get; set; }


        public string DayLeaveType { get; set; }
        public string LeavePurpose { get; set; }


        public string ApprovalRemarks { get; set; }
        public string CancelRemarks { get; set; }
        public string RejectedRemarks { get; set; }

    }
}
