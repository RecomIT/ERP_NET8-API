using Shared.OtherModels.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Employee.Filter.Organizational
{
    public class InternalDesignation_Filter : Sortparam
    {
        public string InternalDesignationId { get; set; }
        public string InternalDesignationName { get; set; }
        public string IsActive { get; set; }
        public string Remarks { get; set; }
    }
}
