using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using System.Threading.Tasks;

namespace BLL.Administration.Interface
{
    public interface IGoogleAuthenticatorBusiness
    {
        Task<object> GenerateQRcodeAsync(string sendToEmail, AppUser appUser);
        Task<ExecutionStatus> TwoFactorAuthenticate(string token, bool sendToEmail, AppUser appUser);
    }
}
