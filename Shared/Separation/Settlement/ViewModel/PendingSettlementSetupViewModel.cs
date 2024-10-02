using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Separation.Settlement.ViewModel
{
    public class PendingSettlementSetupViewModel
    {
        public int ResignationRequestId { get; set; }
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }

    }
}
