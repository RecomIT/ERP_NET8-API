using System.Collections.Generic;

namespace Shared.Payroll.DTO.Tax
{
    public class TaxSettingDTO
    {
        public TaxSettingDTO()
        {
            IncomeTaxSetting = new IncomeTaxSettingDTO();
            TaxExemptionSettings = new List<TaxExemptionSettingDTO>();
            TaxInvestmentSettings = new List<TaxInvestmentSettingDTO>();
        }
        public IncomeTaxSettingDTO IncomeTaxSetting { get; set; }
        public List<TaxExemptionSettingDTO> TaxExemptionSettings { get; set; }
        public List<TaxInvestmentSettingDTO> TaxInvestmentSettings { get; set; }
    }
}
