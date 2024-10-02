using Shared.Asset.DTO.Setting;
using Shared.Asset.Filter.Setting;
using Shared.Asset.ViewModel.Setting;
using Shared.OtherModels.DataService;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;

namespace BLL.Asset.Interface.Setting
{
    public interface IVendorBusiness
    {
        Task<IEnumerable<VendorViewModel>> GetVendorAsync(Vendor_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveVendorAsync(Vendor_DTO model, AppUser user);
        Task<ExecutionStatus> ValidatorVendorAsync(Vendor_DTO model, AppUser user);
        Task<IEnumerable<Dropdown>> GetVendorDropdownAsync(Vendor_Filter filter, AppUser user);
    }
}
