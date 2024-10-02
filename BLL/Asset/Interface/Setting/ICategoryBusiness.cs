
using Shared.Asset.DTO.Setting;
using Shared.Asset.Filter.Setting;
using Shared.Asset.ViewModel.Setting;
using Shared.OtherModels.DataService;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;


namespace BLL.Asset.Interface.Setting
{
    public interface ICategoryBusiness
    {
        Task<IEnumerable<CategoryViewModel>> GetCategoryAsync(Category_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveCategoryAsync(Category_DTO model, AppUser user);
        Task<ExecutionStatus> ValidatorCategoryAsync(Category_DTO model, AppUser user);

        Task<IEnumerable<Dropdown>> GetCategoryDropdownAsync(Category_Filter filter, AppUser user);
     

    }
}
