using DAL.Repository.Base.Interface;
using Shared.Leave.Domain.Balance;
using Shared.Leave.ViewModel.Balance;
using Shared.OtherModels.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repository.Leave.Interface
{
    public interface ILeaveBalanceRepository : IDapperBaseRepository<EmployeeLeaveBalance>
    {
        Task<EmployeeLeaveBalance> GetEmployeeLeaveBalanceOfLeaveTypeAsync(long employeeId, long leaveTypeId, string appliedFromDate, string appliedToDate, AppUser user);
        Task<IEnumerable<LeaveBalanceViewModel>> GetLeaveBalanceAsync(long employeeId, AppUser user);
        Task UpdateApprovedLeaveToAvailed(AppUser user);
    }
}
