
using Shared.Asset.DTO.IT;
using Shared.Asset.Filter.IT;
using Shared.Asset.ViewModel.IT;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;


namespace BLL.Asset.Interface.IT
{
    public interface IITSupportBusiness
    {
        Task<ExecutionStatus> UpdateProductAsync(ITSupport_DTO model, AppUser user);
        Task<DBResponse<ITSupportViewModel>> GetAssetAsync(ITSupport_Filter filter, AppUser user);     
    }
}
