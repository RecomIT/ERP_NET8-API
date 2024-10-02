
using Shared.Leave.Filter.Type;
using Shared.Leave.ViewModel.Setting;
using Shared.Leave.ViewModel.Type;

namespace BLL.Leave.Interface.Type
{
    public interface ILeaveTypeRepo
    {
        Task<IEnumerable<Select2OptionViewModel>> GetSelect2LeaveTypesAsync();
        Task<IEnumerable<Select2OptionViewModel>> GetSelect2LeaveTypesAsync(LeaveType_Filter filter);
        Task<IEnumerable<LeaveTypeViewModel>> GetLeaveTypesAsync(LeaveType_Filter filter);
        Task<IEnumerable<LeaveTypesWithSettings>> GetLeaveTypesWithSettingsAsync(LeaveType_Filter filter);


        // Encashable Leave Type
        Task<IEnumerable<Select2OptionViewModel>> GetSelect2EncashableLeaveTypesAsync();
        Task<IEnumerable<EncashableLeaveTypeSettingViewModel>> GetEncashableLeaveSettingsAsync(LeaveType_Filter filter);

    }
}
