using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Dashboard.CommonDashboard.CompanyEvents
{
    public class CompnayHolidayOrEventViewModel
    {
        public string TableName { get; set; }
        public int EventId { get; set; }
        public string EventTitle { get; set; }
        public string Date { get; set; }
        public string EventTime { get; set; }
        public string Description { get; set; }
    }
}
