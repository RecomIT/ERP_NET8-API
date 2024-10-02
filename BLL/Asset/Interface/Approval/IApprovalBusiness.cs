

using Shared.Asset.ViewModel.Email;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;


namespace BLL.Asset.Interface.Approval
{
    public interface IApprovalBusiness
    {
        Task<ExecutionStatus> ApprovedAssetAsync(long assetId, long assigningId, string activeTab, AppUser user);
        Task<IEnumerable<EmailSendViewModel>> EmailSendAsync(long employeeId, string sendingType, AppUser user);
    }
}
