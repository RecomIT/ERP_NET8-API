using Shared.Control_Panel.DTO.GoogleAuthenticator;
using Shared.OtherModels.User;

namespace BLL.Administration.Interface
{
    public interface IEmailFor2FABusiness
    {

        Task<(bool Success, string Message)> SendEmailAsync(string email, string code, AppUser appUser);
        Task<EmployeeDTO> GetEmployeeDetailsById(long? empId, AppUser user);
    }
}
