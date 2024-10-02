using System.Collections.Generic;


namespace Shared.Payroll.ViewModel.Tax
{
    public class TaxSetting
    {
        public TaxSetting()
        {
            IncomeTaxSetting = new IncomeTaxSettingViewModel();
            TaxExemptionSettings = new List<TaxExemptionSettingViewModel>();
            TaxInvestmentSettings = new List<TaxInvestmentSettingViewModel>();
        }
        public IncomeTaxSettingViewModel IncomeTaxSetting { get; set; }
        public List<TaxExemptionSettingViewModel> TaxExemptionSettings { get; set; }
        public List<TaxInvestmentSettingViewModel> TaxInvestmentSettings { get; set; }
    }
}
