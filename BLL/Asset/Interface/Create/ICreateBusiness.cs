
using Shared.Asset.DTO.Create;
using Shared.Asset.Filter.Create;
using Shared.Asset.ViewModel.Create;
using Shared.Asset.ViewModel.Email;
using Shared.OtherModels.DataService;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;


namespace BLL.Asset.Interface.Create
{
    public interface ICreateBusiness
    {
        Task<DBResponse<CreateViewModel>> GetAssetAsync(Create_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveAssetAsync(Create_DTO model, AppUser user);
        Task<ExecutionStatus> ValidatorAssetAsync(Create_DTO model, AppUser user);
        Task<IEnumerable<Dropdown>> GetAssetDropdownAsync(Create_Filter filter, AppUser user);
        Task<IEnumerable<CreateViewModel>> AssetDropdownAsync(Create_Filter filter, AppUser user);

      

        Task<ExecutionStatus> ValidatorUploadExcelAsync(List<UploadFile_DTO> upload, AppUser user);
        Task<ExecutionStatus> UploadExcelAsync(List<UploadFile_DTO> upload, AppUser user); 
        Task<IEnumerable<ProductViewModel>> GetProductAsync(Product_Filter filter, AppUser user);
        Task<DBResponse<CreateViewModel>> GetAssetDetailsAsync(Create_Filter filter, AppUser user);
        Task<IEnumerable<EmailSendViewModel>> EmailSendAsync(AppUser user);
        Task<ExecutionStatus> ApprovedAssetAsync(long assetId, long assigningId, string activeTab, AppUser user);
    }
}
