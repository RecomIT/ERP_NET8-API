using Shared.Employee.DTO.Account;
using Shared.Employee.Filter.Account;
using Shared.Employee.ViewModel.Account;
using Shared.OtherModels.DataService;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;

namespace BLL.Employee.Interface.Account
{
    public interface IBankBusiness
    {
        Task<IEnumerable<BankViewModel>> GetBanksAsync(Bank_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveBankAsync(BankDTO model, AppUser user);
        Task<ExecutionStatus> ValidatorBankAsync(BankDTO model, AppUser user);
        Task<IEnumerable<Dropdown>> GetBankDropdownAsync(AppUser user);
    }
}
