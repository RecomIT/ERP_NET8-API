using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Dashboard.CommonDashboard.ViewModels.Leave
{
    public class MyLeaveSummaryViewModel
    {
        public int LeaveTypeId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
        public string LeaveTypeName { get; set; }
        public string LeaveTypeShortName { get; set; }
        public int YearlyTotalLeaveAvailed { get; set; }
        public double MonthlyTotalLeaveAvailed { get; set; }
        public string LeaveDates { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string YearMonth { get; set; }
        public string Period { get; set; }
        public string LeavePeriod { get; set; }

    }

}
