

using Shared.Asset.DTO.Setting;
using Shared.Asset.Filter.Setting;
using Shared.Asset.ViewModel.Setting;
using Shared.OtherModels.DataService;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;


namespace BLL.Asset.Interface.Setting
{
    public interface IStoreBusiness
    {
        Task<IEnumerable<StoreViewModel>> GetStoreAsync(Store_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveStoreAsync(Store_DTO model, AppUser user);
        Task<ExecutionStatus> ValidatorStoreAsync(Store_DTO model, AppUser user);
        Task<IEnumerable<Dropdown>> GetStoreDropdownAsync(Store_Filter filter, AppUser user);

    }
}
