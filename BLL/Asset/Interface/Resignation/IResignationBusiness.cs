
using Shared.Asset.DTO.Resignation;
using Shared.Asset.Filter.Resignation;
using Shared.Asset.ViewModel.Resignation;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;


namespace BLL.Asset.Interface.Resignation
{
    public interface IResignationBusiness
    {        
        Task<DBResponse<ResignationViewModel>> GetEmployeeResignationAsync(Resignation_Filter filter, AppUser user);
        Task<DBResponse<AssetListViewModel>> GetAssignedDataAsync(AssetList_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveAssetAsync(Resignation_DTO model, AppUser user); 
    }
}
