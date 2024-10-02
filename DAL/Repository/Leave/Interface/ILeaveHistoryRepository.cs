using DAL.Repository.Base.Interface;
using Shared.Leave.Domain.History;
using Shared.Leave.ViewModel.History;
using Shared.OtherModels.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repository.Leave.Interface
{
    public interface ILeaveHistoryRepository : IDapperBaseRepository<EmployeeLeaveHistory>
    {
        Task<IEnumerable<EmployeeLeaveHistoryInfoViewModel>> GetLeaveHistoryByIdAsync(long requestId, AppUser user);
    }
}
