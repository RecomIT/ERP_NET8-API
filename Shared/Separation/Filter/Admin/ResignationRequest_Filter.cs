using Shared.OtherModels.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Separation.Filter.Admin
{
    public class ResignationRequest_Filter : Sortparam
    {
        public int? EmployeeId { get; set; }
        public int? ResignationRequestId { get; set; }

    }
}
