
using Shared.Leave.ViewModel.Encashment;
using Shared.OtherModels.User;

namespace BLL.Leave.Interface.Encashment
{
    public interface ILeaveEncashmentBusiness
    {
        Task<EncashmentBalanceViewModel> GetTotalLeaveBalanceAsync(dynamic filter);


    }
}
