using System.Collections.Generic;

namespace Shared.Payroll.Process.Tax
{
    public class TaxProcessedInfo
    {
        public TaxProcessedInfo()
        {
            EmployeeTaxProcess = new TaxProcessInfo();
            EmployeeTaxProcessDetails = new List<TaxDetailInTaxProcess>();
            EmployeeTaxProcessSlabs = new List<TaxProcessSlab>();
        }

        public TaxProcessInfo EmployeeTaxProcess { get; set; }
        public List<TaxDetailInTaxProcess> EmployeeTaxProcessDetails { get; set; }
        public List<TaxProcessSlab> EmployeeTaxProcessSlabs { get; set; }
    }
}
