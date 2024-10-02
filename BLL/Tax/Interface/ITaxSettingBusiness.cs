using Shared.Payroll.DTO.Tax;
using Shared.OtherModels.User;
using Shared.Payroll.Process.Tax;
using Shared.OtherModels.Response;
using Shared.Payroll.ViewModel.Tax;

namespace BLL.Tax.Interface
{
    public interface ITaxSettingBusiness
    {
        Task<ExecutionStatus> ValidateIncomeTaxSettingAsync(TaxSettingDTO setting, AppUser user);
        Task<ExecutionStatus> SaveTaxSettingAsync(TaxSettingDTO setting, AppUser user);
        Task<IEnumerable<IncomeTaxSettingViewModel>> GetTaxSettingsAsync(long? IncomeTaxSettingId, long? FiscalYearId, string ImpliedCondition, AppUser user);
        Task<TaxSetting> GetTaxSettingAsync(long? IncomeTaxSettingId, AppUser user);
        Task<TaxSettingInTaxProcess> GetTaxSettingByFiscalYearIdAsync(long FiscalYearId, AppUser user);
    }
}
