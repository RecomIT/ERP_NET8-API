
using Shared.Employee.DTO.Account;
using Shared.Employee.Filter.Account;
using Shared.Employee.ViewModel.Account;
using Shared.OtherModels.DataService;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;

namespace BLL.Employee.Interface.Account
{
    public interface IBankBranchBusiness
    {
        Task<IEnumerable<BankBranchViewModel>> GetBankBranchesAsync(BankBranch_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveBankBranchAsync(BankBranchDTO model, AppUser user);
        Task<ExecutionStatus> ValidatorBankBranchAsync(BankBranchDTO model, AppUser user);
        Task<IEnumerable<Dropdown>> GetBankBranchDropdownAsync(BankBranch_Filter filter, AppUser user);
    }
}
