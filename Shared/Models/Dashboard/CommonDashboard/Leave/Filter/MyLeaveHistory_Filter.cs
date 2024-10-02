using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Dashboard.CommonDashboard.Leave.Filter
{
    public class MyLeaveHistory_Filter
    {
        public int FromYear { get; set; }
        public int? ToYear { get; set; }
        public int FromMonth { get; set; }
        public int? ToMonth { get; set; }
    }
}
