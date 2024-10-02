using Shared.Attendance.ViewModel.Attendance;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Attendance.Interface.Attendance
{
    public interface IAttendanceProcessBusiness
    {
        Task<ExecutionStatus> LockAttendanceProcessAsync(AttendanceProcessLockUnlock model, AppUser user);
        Task<ExecutionStatus> UnLockAttendanceProcessAsync(AttendanceProcessLockUnlock model, AppUser user);
        Task<ExecutionStatus> ValidateAttendanceProcessAsync(short month, short year, AppUser user);
        Task<ExecutionStatus> AttendanceProcessAsync(AttendanceProcessViewModel model, AppUser user);
        Task<IEnumerable<AttendanceProcessViewModel>> GetAttendanceProcessInfosAsync(short? month, short? year, AppUser user);
        Task<ExecutionStatus> UploadRowAttendanceData(List<UploadAttendanceViewModel> attendances, AppUser user);
    }
}
