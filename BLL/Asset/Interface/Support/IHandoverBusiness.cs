
using Shared.Asset.DTO.Support;
using Shared.Asset.Filter.Support;
using Shared.Asset.ViewModel.Support;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;


namespace BLL.Asset.Interface.Support
{
    public interface IHandoverBusiness
    {        
        Task<ExecutionStatus> SaveHandoverAsync(Handover_DTO model, AppUser user);        
        Task<DBResponse<HandoverViewModel>> GetHandoverDataAsync(Handover_Filter filter, AppUser user);      
        Task<IEnumerable<HandoverViewModel>> GetAssignedDataAsync(Handover_Filter filter, AppUser user);
    }
}
