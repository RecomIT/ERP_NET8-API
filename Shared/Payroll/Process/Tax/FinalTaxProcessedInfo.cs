using Shared.Payroll.Domain.Tax;

namespace Shared.Payroll.Process.Tax
{
    public class FinalTaxProcessedInfo
    {
        public FinalTaxProcessedInfo()
        {
            FinalTaxProcess = new FinalTaxProcess();
            FinalTaxProcessDetails = new List<FinalTaxProcessDetail>();
            FinalTaxProcessSlabs = new List<FinalTaxProcessSlab>();
        }

        public FinalTaxProcess FinalTaxProcess { get; set; }
        public List<FinalTaxProcessDetail> FinalTaxProcessDetails { get; set; }
        public List<FinalTaxProcessSlab> FinalTaxProcessSlabs { get; set; }
    }
}
