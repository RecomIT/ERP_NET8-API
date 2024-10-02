using Shared.Leave.Domain.Balance;
using Shared.Leave.DTO.Balance;
using Shared.Leave.Filter.Report;
using Shared.Leave.ViewModel.Balance;
using Shared.OtherModels.DataService;
using Shared.OtherModels.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Leave.Interface.Balance
{
    public interface IEmployeeLeaveBalanceBusiness
    {
        Task<EmployeeLeaveBalance> GetEmployeeLeaveBalanceOfLeaveTypeAsync(long employeeId, long leaveTypeId, string leavePeriodStart, string leavePeriodEnd, AppUser user);
        Task<IEnumerable<EmployeeLeaveBalanceViewModel>> GetEmployeeLeaveBalancesAsync(LeaveBalance_Filter filter, AppUser user);
        Task<IEnumerable<Select2Dropdown>> GetEmployeeLeaveBalancesDropdownAsync(long employeeId, AppUser user);
        Task<IEnumerable<Select2Dropdown>> GetEmployeeLeaveBalancesDropdownInEditAsync(long employeeLeaveRequestId, long employeeId, AppUser user);
        Task<IEnumerable<LeaveBalanceViewModel>> GetLeaveBalanceAsync(long employeeId, AppUser user);



        Task<PaginatedList<EmployeeLeaveBalanceDto>> GetEmployeeLeaveBalances(dynamic filter, AppUser user);
        Task<List<EmployeeLeaveBalanceDtoForExcel>> GetAllEmployeeLeaveBalances(AppUser user);

        Task<List<Dictionary<string, object>>> GetAllEmployeeLeaveBalance(AppUser user);

        Task<object> SaveLeaveBalanceAsync(dynamic filter, AppUser user);

    }
}
