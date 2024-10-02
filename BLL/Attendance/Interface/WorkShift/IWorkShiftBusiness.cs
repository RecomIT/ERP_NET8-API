using System.Threading.Tasks;
using Shared.OtherModels.User;
using Shared.OtherModels.DataService;
using Shared.OtherModels.Response;
using System.Collections.Generic;
using Shared.Attendance.ViewModel.Workshift;
using Shared.Attendance.DTO.Workshift;
using Shared.Attendance.Filter.Shift;

namespace BLL.Attendance.Interface.WorkShift
{
    public interface IWorkShiftBusiness
    {
        Task<WorkShiftViewModel> GetWorkShiftsByIdAsync(long workShiftId, AppUser appUser);
        Task<IEnumerable<WorkShiftViewModel>> GetWorkShiftsAsync(WorkShift_Filter filter, AppUser appUser);
        Task<ExecutionStatus> SaveWorkShiftAsync(WorkShiftDTO model, List<string> Weekends, AppUser appUser);
        Task<ExecutionStatus> SaveWorkShiftChecking(WorkShiftCheckingDTO model, AppUser appUser);
        Task<ExecutionStatus> WorkShiftValidatorAsync(WorkShiftDTO model, AppUser appUser);
        Task<IEnumerable<Select2Dropdown>> GetWorkShiftDropdownAsync(AppUser user);
    }
}
