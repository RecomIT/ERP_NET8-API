

using Shared.Asset.ViewModel.Dashboard;
using Shared.OtherModels.User;


namespace BLL.Asset.Interface.Dashboard
{
    public interface IEmployeeBusiness    {
        Task<IEnumerable<EmployeeViewModel>> GetAssetAsync(AppUser user);
    }
}
