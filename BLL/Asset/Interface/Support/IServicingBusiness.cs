
using Shared.Asset.DTO.Support;
using Shared.Asset.Filter.Support;
using Shared.Asset.ViewModel.Support;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;


namespace BLL.Asset.Interface.Support
{
    public interface IServicingBusiness
    {        
        Task<ExecutionStatus> SaveServicingAsync(Servicing_DTO model, AppUser user);        
        Task<DBResponse<ServicingViewModel>> GetServicingDataAsync(Servicing_Filter filter, AppUser user);    
        Task<DBResponse<ReceivedViewModel>> GetReceivedAssetAsync(Servicing_Filter filter, AppUser user);
    }
}
