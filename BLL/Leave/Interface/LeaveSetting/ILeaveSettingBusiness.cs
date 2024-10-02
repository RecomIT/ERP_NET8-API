using System.Threading.Tasks;
using Shared.OtherModels.User;
using System.Collections.Generic;
using Shared.OtherModels.Response;
using Shared.Leave.DTO.Setup;
using Shared.Leave.Filter.Request;
using Shared.Leave.Filter.Setup;
using Shared.Leave.ViewModel.Setup;

namespace BLL.Leave.Interface.LeaveSetting
{
    public interface ILeaveSettingBusiness
    {
        Task<IEnumerable<LeaveSettingViewModel>> GetLeaveSettingsAsync(LeaveSetting_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveLeaveSettingAsync(LeaveSettingDTO model, AppUser user);
        Task<ExecutionStatus> LeaveSettingValidatorAsync(LeaveSettingDTO model, AppUser user);
        Task<ExecutionStatus> DeleteLeaveSettingAsync(long leaveSettingId, AppUser user);
        Task<IEnumerable<LeaveSettingInfoDTO>> GetLeavePeriodAsync(long employeeId, AppUser user);
        Task<IEnumerable<LeaveSettingInfoDTO>> GetLeaveTypeSettingAsync(long leaveTypeId, long? employeeId, AppUser user);
        Task<ExecutionStatus> GetTotalRequestDaysAsync(TotalRequestDays_Filter filter, AppUser user);
        Task<LeavePeriodRange> GetLeavePeriodDateRange(DateTime? joiningDate, int leaveYear, AppUser user);

    }
}
