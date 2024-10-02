using DAL.Repository.Base.Interface;
using Shared.Leave.Domain.Setup;
using Shared.Leave.DTO.Setup;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using System.Threading.Tasks;

namespace DAL.Repository.Leave.Interface
{
    public interface ILeaveSettingRepository : IDapperBaseRepository<LeaveSetting>
    {
        Task<ExecutionStatus> SaveAsync(LeaveSettingDTO model, AppUser user);
    }
}
