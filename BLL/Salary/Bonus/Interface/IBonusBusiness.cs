using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Payroll.Filter.Bonus;
using Shared.OtherModels.DataService;
using Shared.Payroll.ViewModel.Bonus;

namespace BLL.Salary.Bonus.Interface
{
    public interface IBonusBusiness
    {
        Task<ExecutionStatus> SaveBonusAsync(BonusViewModel bonus, AppUser user);
        Task<IEnumerable<BonusViewModel>> GetBonusesAsync(string BonusName, long? bonusId, AppUser user);
        Task<ExecutionStatus> SaveBonusConfigAsync(BonusConfigViewModel bonus, AppUser user);
        Task<IEnumerable<BonusConfigViewModel>> GetBonusConfigsAsync(BonusQuery filter, AppUser user);
        Task<IEnumerable<Dropdown>> GetBonusExtensionAsync(AppUser user);
        Task<IEnumerable<Dropdown>> GetBonusAndConfigInThisFiscalYearExtensionAsync(AppUser user);
        Task<IEnumerable<Select2Dropdown>> GetLFAYearlyAllowanceExtensionAsync(long? allowanceNameId, AppUser appUser);
    }
}
