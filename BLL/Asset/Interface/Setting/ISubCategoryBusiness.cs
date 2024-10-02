
using Shared.Asset.DTO.Setting;
using Shared.Asset.Filter.Setting;
using Shared.Asset.ViewModel.Setting;
using Shared.OtherModels.DataService;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;

namespace BLL.Asset.Interface.Setting
{
    public interface ISubCategoryBusiness
    {
        Task<IEnumerable<SubCategoryViewModel>> GetSubCategoryAsync(SubCategory_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveSubCategoryAsync(SubCategory_DTO model, AppUser user);
        Task<ExecutionStatus> ValidatorSubCategoryAsync(SubCategory_DTO model, AppUser user);
        Task<IEnumerable<Dropdown>> GetSubCategoryDropdownAsync(SubCategory_Filter filter, AppUser user);
    }
}
