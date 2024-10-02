
using Shared.Asset.DTO.Support;
using Shared.Asset.Filter.Support;
using Shared.Asset.ViewModel.Support;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;


namespace BLL.Asset.Interface.Support
{
    public interface IRepairedBusiness
    {        
        Task<ExecutionStatus> SaveRepairedAsync(Repaired_DTO model, AppUser user);        
        Task<DBResponse<RepairedViewModel>> GetRepairedDataAsync(Servicing_Filter filter, AppUser user);    
        Task<DBResponse<ServicingViewModel>> GetServicingAssetAsync(Servicing_Filter filter, AppUser user);
    }
}
