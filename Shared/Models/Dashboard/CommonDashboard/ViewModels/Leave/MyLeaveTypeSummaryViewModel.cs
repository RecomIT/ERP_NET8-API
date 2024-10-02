using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Dashboard.CommonDashboard.ViewModels.Leave
{
    public class MyLeaveTypeSummaryViewModel
    {
        public string LeaveTypeName { get; set; }
        public double Sanctioned { get; set; }
        public double Availed { get; set; }
        public double Pending { get; set; }
        public double Approved { get; set; }
        public double Cancelled { get; set; }
        public double Rejected { get; set; }
        public double Balance { get; set; }
    }
}
