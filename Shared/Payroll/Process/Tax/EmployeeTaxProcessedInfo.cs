

using System.Collections.Generic;
using Shared.Payroll.Domain.Tax;

namespace Shared.Payroll.Process.Tax
{
    public class EmployeeTaxProcessedInfo
    {
        public EmployeeTaxProcessedInfo()
        {
            EmployeeTaxProcess = new EmployeeTaxProcess();
            EmployeeTaxProcessDetails = new List<EmployeeTaxProcessDetail>();
            EmployeeTaxProcessSlabs = new List<EmployeeTaxProcessSlab>();
        }

        public EmployeeTaxProcess EmployeeTaxProcess { get; set; }
        public List<EmployeeTaxProcessDetail> EmployeeTaxProcessDetails { get; set; }
        public List<EmployeeTaxProcessSlab> EmployeeTaxProcessSlabs { get; set; }
    }
}
