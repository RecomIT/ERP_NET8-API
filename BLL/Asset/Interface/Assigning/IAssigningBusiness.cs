
using Shared.Asset.DTO.Assigning;
using Shared.Asset.Filter.Assigning;
using Shared.Asset.Filter.Create;
using Shared.Asset.ViewModel.Assigning;
using Shared.Asset.ViewModel.Create;
using Shared.Asset.ViewModel.Email;
using Shared.OtherModels.DataService;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;


namespace BLL.Asset.Interface.Assigning
{
    public interface IAssigningBusiness
    {
        Task<DBResponse<AssigningViewModel>> GetAssignedDataAsync(Assigning_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveAssetAssigningAsync(Assigning_DTO model, AppUser user);
        Task<ExecutionStatus> ValidatorAssetAssigningAsync(Assigning_DTO model, AppUser user);
        Task<ExecutionStatus> ApprovedAssetAsync(long assetId, long assigningId, string activeTab, AppUser user);
        Task<DBResponse<CreateViewModel>> GetAssetAsync(Create_Filter filter, AppUser user);
        Task<IEnumerable<ProductViewModel>> GetProductAsync(Product_Filter filter, AppUser user);
        Task<IEnumerable<EmailSendViewModel>> EmailSendAsync(long employeeId, AppUser user);
        Task<IEnumerable<Dropdown>> GetProductDropdownAsync(Product_Filter filter, AppUser user);
        Task<IEnumerable<ProductViewModel>> ProductDropdownAsync(Product_Filter filter, AppUser user);
    }
}
