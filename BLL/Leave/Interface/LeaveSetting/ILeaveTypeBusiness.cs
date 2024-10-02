using Shared.Leave.Domain.Setup;
using Shared.Leave.DTO.Setup;
using Shared.Leave.ViewModel.Setup;
using Shared.OtherModels.DataService;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Leave.Interface.LeaveSetting
{
    public interface ILeaveTypeBusiness
    {
        Task<IEnumerable<LeaveTypeViewModel>> GetLeaveTypesAsync(LeaveType_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveLeaveTypeAsync(LeaveTypeDTO model, AppUser user);
        Task<ExecutionStatus> LeaveTypeValidatorAsync(LeaveTypeDTO model, AppUser user);
        Task<ExecutionStatus> DeleteLeaveTypeAsync(long leaveTypeId, AppUser user);
        Task<IEnumerable<Select2Dropdown>> GetLeaveTypesDropdownAsync(AppUser user);
        Task<LeaveType> GetLeaveTypeById(long id, AppUser user);
    }
}
