using Shared.Control_Panel.Domain;
using Shared.OtherModels.User;
using System.Threading.Tasks;

namespace BLL.Base.Interface
{
    public interface IModuleConfig
    {
        Task<PayrollModuleConfig> PayrollModuleConfig(AppUser user);
        Task<HRModuleConfig> HRModuleConfig(AppUser user);
        Task<PFModuleConfig> PFModuleConfig(AppUser user);
    }
}
