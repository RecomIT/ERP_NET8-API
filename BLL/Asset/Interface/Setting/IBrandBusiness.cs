using Shared.Asset.DTO.Setting;
using Shared.Asset.Filter.Setting;
using Shared.Asset.ViewModel.Setting;
using Shared.OtherModels.DataService;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;

namespace BLL.Asset.Interface.Setting
{
    public interface IBrandBusiness
    {
        Task<IEnumerable<BrandViewModel>> GetBrandAsync(Brand_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveBrandAsync(Brand_DTO model, AppUser user);
        Task<ExecutionStatus> ValidatorBrandAsync(Brand_DTO model, AppUser user);

        Task<IEnumerable<Dropdown>> GetBrandDropdownAsync(Brand_Filter filter, AppUser user);
        Task<IEnumerable<BrandViewModel>> BrandDropdownAsync(Brand_Filter filter, AppUser user);
    }
}
