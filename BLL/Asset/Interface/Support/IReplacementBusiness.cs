
using Shared.Asset.DTO.Support;
using Shared.Asset.Filter.Support;
using Shared.Asset.ViewModel.Support;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;


namespace BLL.Asset.Interface.Support
{
    public interface IReplacementBusiness
    {
        Task<ExecutionStatus> UpdateProductAsync(Replacement_DTO model, AppUser user);
        Task<ExecutionStatus> SaveReplacementAsync(Replacement_DTO model, AppUser user);
        Task<ExecutionStatus> SaveReceivedAsync(Received_DTO model, AppUser user);
        Task<DBResponse<ReplacementViewModel>> GetAssetAsync(Replacement_Filter filter, AppUser user);     
    }
}
