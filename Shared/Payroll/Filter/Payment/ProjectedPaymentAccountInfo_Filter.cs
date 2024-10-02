using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Payroll.Filter.Payment
{
    public class ProjectedPaymentAccountInfo_Filter
    {
        public string EmployeeId { get; set; }
        public string PaymentMode { get; set; }
        public decimal? COCPercentage { get; set; }
    }
}
