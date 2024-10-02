
using Shared.Employee.Domain.Account;
using Shared.Employee.DTO.Account;
using Shared.Employee.Filter.Account;
using Shared.Employee.ViewModel.Account;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;


namespace BLL.Employee.Interface.Account
{
    public interface IAccountInfoBusiness
    {
        Task<ExecutionStatus> SaveEmployeeAccountInfoAsync(EmployeeAccountInfoDTO model, AppUser user);
        Task<ExecutionStatus> EmployeeAccountInfoValidatorAsync(EmployeeAccountInfoDTO model, AppUser user);
        Task<DBResponse<EmployeeAccountInfoViewModel>> GetEmployeeAccountInfosAsync(EmployeeAccount_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveApprovalOfEmployeeAccountAsync(AccountInfoStatusDTO model, AppUser user);
        Task<ExecutionStatus> UploadAccountInfoAsync(List<EmployeeAccountInfoViewModel> models, AppUser user);
        Task<EmployeeAccountInfoViewModel> GetAccountActivationInfoBeforeDate(long employeeId, string before_date, AppUser user);
        Task<EmployeeAccountInfo> GetActiveAccountInfoByEmployeeId(long employeeId, AppUser user);
        Task<EmployeeAccountInfo> GetPendingAccountInfoByEmployeeId(long employeeId, AppUser user);
        Task<bool> IsAccountNumberAlreadyActive(string accountNumber, AppUser user);
    }
}
