using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Dashboard.CommonDashboard.CompanyEvents
{
    public class CompanyEventModel
    {
        public int Id { get; set; }
        public string EventTitle { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public TimeSpan EventTime { get; set; }
        public string Action { get; set; }
    }
}
