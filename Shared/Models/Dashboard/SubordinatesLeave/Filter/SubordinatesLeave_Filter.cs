using Shared.OtherModels.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Dashboard.SubordinatesLeave.Filter
{
    public class SubordinatesLeave_Filter:Sortparam
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public long? EmployeeId { get; set; }
    }
}
