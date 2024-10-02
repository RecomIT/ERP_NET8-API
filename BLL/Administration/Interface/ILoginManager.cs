using Shared.OtherModels.Response;
using Shared.Control_Panel.ViewModels;

namespace BLL.Administration.Interface
{
    public interface ILoginManager
    {
        Task<AppUserLoggedInfo> GetAppUserLoggedInfo2Async(string username);
        Task<AppUserLoggedInfo> GetAppUserLoggedInfoAsync(string username);
        Task<bool> IsEmailExistAsync(string email);
        Task<ExecutionStatus> UserForgetPasswordOTPResquestAsync(OTPRequestsViewModel model);
        Task<ExecutionStatus> UserForgetPasswordOTPVerificationAsync(OTPVerificationViewModel model);
        Task<EmailSettingObject> EmailSettings(string EmaliFor);
        Task<AppUserLoggedInfo> GetAppUserEmployeeInfoAsync(long employeeId, long companyId, long organizationId, string database);
        Task<IEnumerable<loginViewModel>> GetLoginInfosAsync(long companyId);
    }
}
