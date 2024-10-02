
using Shared.Asset.Filter.Report;
using Shared.Asset.ViewModel.Dashboard;
using Shared.OtherModels.User;


namespace BLL.Asset.Interface.Dashboard
{
    public interface IAdminBusiness    {
        Task<object> GetAssetCreationDataAsync(AppUser user);
        Task<object> GetAssetAssigningDataAsync(AppUser user);
        Task<IEnumerable<AdminViewModel>> GetAssetAsync(Report_Filter filter, AppUser user);

    }
}
