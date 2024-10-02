using Shared.Payroll.Domain.Tax;

namespace Shared.Payroll.Process.Tax
{
    public class TaxSettingInTaxProcess
    {
        public TaxSettingInTaxProcess()
        {
            IncomeTaxSetting = new IncomeTaxSetting();
            TaxExemptionSettings = new List<TaxExemptionSetting>();
            TaxInvestmentSetting = new TaxInvestmentSetting();
        }
        public IncomeTaxSetting IncomeTaxSetting { get; set; }
        public List<TaxExemptionSetting> TaxExemptionSettings { get; set; }
        public TaxInvestmentSetting TaxInvestmentSetting { get; set; }
    }
}
